using FIFA_API.Exceptions;
using FIFA_API.Models;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace FIFA_API.Controllers
{
    public partial class CommandesController
    {
        #region User commands
        [HttpGet("UserCommands/{idUtilisateur}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Commande>>> GetUserCommands(int idUtilisateur, [FromQuery] bool? desc)
        {
            Utilisateur? user = await _context.Utilisateurs.GetByIdAsync(idUtilisateur);
            if (user is null) return NotFound();

            return Ok(SortedCommands(user.Commandes, desc));
        }

        [HttpGet("MyCommands")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<IEnumerable<Commande>>> GetMyCommands([FromQuery] bool? desc)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return Ok(SortedCommands(user.Commandes, desc));
        }

        [HttpGet("MyCommands/{idcommande}")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<Commande>> GetMyCommand(int idCommande)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            Commande? commande = await _context.Commandes.GetByIdAsync(idCommande);
            if (commande is null || commande.IdUtilisateur != user.Id) return NotFound();

            return Ok(commande);
        }

        private static IEnumerable<Commande> SortedCommands(IEnumerable<Commande> commands, bool? desc)
            => desc == true ? commands.OrderBy(c => c.DateCommande) : commands.OrderByDescending(c => c.DateCommande);
        #endregion

        #region Stripe
        [HttpPost("checkout")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<StartStripeSessionResponse>> StartStripeCommand([FromBody] Panier panier, [FromServices] IServiceProvider sp)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            var server = sp.GetRequiredService<IServer>();
            var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

            string? fifApiUrl = serverAddressesFeature?.Addresses.FirstOrDefault();
            if (fifApiUrl is null) return StatusCode(500, new{ Message = "Could not resolve API URL" });

            try
            {
                var url = await Checkout(user, panier, fifApiUrl);
                return Ok(new StartStripeSessionResponse()
                {
                    SessionId = url,
                    PublicKey = _config["Stripe:PublicKey"],
                });
            }
            catch(CommandeException e)
            {
                return BadRequest(new { e.Cause, e.Message });
            }
        }

        [NonAction]
        public async Task<string> Checkout(Utilisateur user, Panier panier, string apiUrl)
        {
            var options = new SessionCreateOptions()
            {
                ClientReferenceId = user.Id.ToString(),
                SuccessUrl = $"{apiUrl}/checkout/success?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = "",
                InvoiceCreation = new() { Enabled = true },

                LineItems = await GetCommandLines(panier),
                ShippingOptions = await GetShippingOptions(),

                Mode = "payment",
                AutomaticTax = new() { Enabled = true },
                PaymentIntentData = new() { SetupFutureUsage = "on_session" },

                BillingAddressCollection = "required",
                ShippingAddressCollection = new() { AllowedCountries = new List<string>() { "FR" } },

                PhoneNumberCollection = new() { Enabled = true },
            };

            if(user.StripeId is not null)
            {
                options.Customer = user.StripeId;
                options.CustomerUpdate = new() { Shipping = "auto" };
            }
            else
            {
                options.CustomerCreation = "always";
                options.CustomerEmail = user.Mail;
            }

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            user.StripeId = session.Customer.Id;
            await _context.SaveChangesAsync();

            return session.Url;
        }

        [NonAction]
        public async Task<List<SessionLineItemOptions>> GetCommandLines(Panier panier)
        {
            var items = new List<SessionLineItemOptions>();

            foreach(var item in panier.Items)
            {
                var variante = await _context.VarianteCouleurProduits.GetByIdAsync(item.IdVCProduit);
                if(variante is null)
                    throw new CommandeException(CommandeExceptionCause.NoVariante, $"Variante inconnue (ID: {item.IdVCProduit})");

                var taille = await _context.TailleProduits.FindAsync(item.IdTaille);
                if (taille is null)
                    throw new CommandeException(CommandeExceptionCause.NoTaille, $"Taille inconnue (ID: {item.IdTaille})");

                var stocks = await _context.StockProduits.FindAsync(item.IdVCProduit, item.IdTaille);
                if(stocks is null)
                    throw new CommandeException(CommandeExceptionCause.NoStocks, $"Stock inconnu (ID_VCP: {item.IdVCProduit}, ID_TPR: {item.IdTaille})");

                if (stocks.Stocks - item.Quantite < 0)
                    throw new CommandeException(CommandeExceptionCause.StocksEmpty, $"Stocks épuisés (ID_VCP: {item.IdVCProduit}, ID_TPR: {item.IdTaille})");

                items.Add(new()
                {
                    PriceData = new()
                    {
                        UnitAmountDecimal = variante.Prix * 100,
                        Currency = "EUR",
                        ProductData = new()
                        {
                            Name = $"{variante.Produit.Titre} | {variante.Couleur.Nom} | {taille.Nom}",
                            Description = variante.Produit.Description,
                            Images = variante.ImageUrls,
                            Metadata = new()
                            {
                                { "IdVCProduit", item.IdVCProduit.ToString() },
                                { "IdTaille", item.IdTaille.ToString() },
                            }
                        }
                    },
                    Quantity = item.Quantite
                });
            }
            return items;
        }

        [NonAction]
        public async Task<List<SessionShippingOptionOptions>> GetShippingOptions()
        {
            var types = await _context.TypeLivraisons.ToListAsync();
            return types.Select(type => new SessionShippingOptionOptions()
            {
                ShippingRateData = new()
                {
                    DisplayName = type.Nom,
                    DeliveryEstimate = new()
                    {
                        Maximum = new()
                        {
                            Unit = "business_days",
                            Value = type.MaxBusinessDays
                        }
                    },
                    FixedAmount = new()
                    {
                        Amount = (long)(type.Prix * 100),
                        Currency = "EUR"
                    },
                    Type = "fixed_amount",
                    Metadata = new() { { "IdTypeLivraison", type.Id.ToString() } }
                }
            }).ToList();
        }

        [HttpGet("checkout/success")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<StripeSuccessResult>> StripeSuccess([FromQuery] string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);
            if (session is null) return NotFound();

            int iduser = int.Parse(session.ClientReferenceId);
            Utilisateur? user = await _context.Utilisateurs.FindAsync(iduser);
            if (user is null) return Unauthorized();

            Commande commande;
            try { commande = await RegisterStripeCommand(session, user); }
            catch(CommandeException e)
            {
                return StatusCode(500, new { e.Cause, e.Message });
            }

            var deliveryEstimate = session.ShippingCost.ShippingRate.DeliveryEstimate;

            return Ok(new StripeSuccessResult()
            {
                IdCommande = commande.Id,
                EstimateMaxValue = deliveryEstimate.Maximum.Value,
                EstimateUnit = deliveryEstimate.Maximum.Unit,
            });
        }

        [NonAction]
        public async Task<Commande> RegisterStripeCommand(Session session, Utilisateur user)
        {
            user.StripeId = session.Customer.Id;

            var adrLivraison = session.ShippingDetails.Address;
            var adrFacturation = session.CustomerDetails.Address;

            Commande commande = new Commande()
            {
                IdUtilisateur = user.Id,
                IdTypeLivraison = int.Parse(session.ShippingCost.ShippingRate.Metadata["IdTypeLivraison"]),
                PrixLivraison = (decimal)session.ShippingCost.AmountTotal / 100,
                UrlFacture = session.Invoice.InvoicePdf,

                VilleLivraison = adrLivraison.City,
                CodePostalLivraison = adrLivraison.PostalCode,
                RueLivraison = adrLivraison.Line1,

                VilleFacturation = adrFacturation.City,
                CodePostalFacturation = adrFacturation.PostalCode,
                RueFacturation = adrFacturation.Line1,
            };

            foreach (var item in session.LineItems)
            {
                int idVCProduit = int.Parse(item.Price.Metadata["IdVCProduit"]);
                int idTaille = int.Parse(item.Price.Metadata["IdTaille"]);

                var stocks = await _context.StockProduits.FindAsync(idVCProduit, idTaille);
                if (stocks is null)
                    throw new CommandeException(CommandeExceptionCause.NoStocks, $"Stock inconnu (ID_VCP: {idVCProduit}, ID_TPR: {idTaille})");

                if (stocks.Stocks - item.Quantity < 0)
                    throw new CommandeException(CommandeExceptionCause.StocksEmpty, $"Stocks épuisés (ID_VCP: {idVCProduit}, ID_TPR: {idTaille})");

                commande.Lignes.Add(new()
                {
                    Commande = commande,
                    IdVCProduit = idVCProduit,
                    IdTaille = idTaille,
                    PrixUnitaire = (decimal)item.Price.UnitAmountDecimal! / 100,
                    Quantite = (int)item.Quantity!
                });
            }

            await _context.Commandes.AddAsync(commande);
            await _context.SaveChangesAsync();

            return commande;
        }
        #endregion
    }
}

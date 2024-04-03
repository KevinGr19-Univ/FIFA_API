using FIFA_API.Authorization;
using FIFA_API.Exceptions;
using FIFA_API.Models;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace FIFA_API.Controllers
{
    public partial class CommandesController
    {
		[HttpPost("checkout")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
		[VerifiedEmail]
		public async Task<ActionResult<StartStripeSessionResponse>> StartStripeCommand([FromBody] Panier panier)
		{
			Utilisateur? user = await this.UtilisateurAsync();
			if (user is null) return Unauthorized();

			try
			{
				var url = await Checkout(user, panier);
				return Ok(new StartStripeSessionResponse()
				{
					SessionId = url,
					PublicKey = _config["Stripe:PublicKey"],
				});
			}
			catch (CommandeException e)
			{
				return BadRequest(new { e.Cause, e.Message });
			}
		}

		[NonAction]
		public async Task<string> Checkout(Utilisateur user, Panier panier)
		{
			var options = new SessionCreateOptions()
			{
				ClientReferenceId = user.Id.ToString(),
				SuccessUrl = panier.SuccessUrl,
				CancelUrl = panier.CancelUrl,
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

			if (user.StripeId is not null)
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

			return session.Url;
		}

		[NonAction]
		public async Task<List<SessionLineItemOptions>> GetCommandLines(Panier panier)
		{
			var items = new List<SessionLineItemOptions>();

			foreach (var item in panier.Items)
			{
				var variante = await _context.VarianteCouleurProduits.GetByIdAsync(item.IdVCProduit);
				if (variante is null)
					throw new CommandeException(CommandeExceptionCause.NoVariante, $"Variante inconnue (ID: {item.IdVCProduit})");

				var taille = await _context.TailleProduits.FindAsync(item.IdTaille);
				if (taille is null)
					throw new CommandeException(CommandeExceptionCause.NoTaille, $"Taille inconnue (ID: {item.IdTaille})");

				var stocks = await _context.StockProduits.FindAsync(item.IdVCProduit, item.IdTaille);
				if (stocks is null)
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
			var types = await _context.TypeLivraisons.Take(5).ToListAsync();
			return types.Select(type => new SessionShippingOptionOptions()
			{
				ShippingRateData = new()
				{
					DisplayName = type.Nom,
					DeliveryEstimate = new()
					{
						Maximum = new()
						{
							Unit = "business_day",
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

		[HttpPost("webhook")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> StripeWebhook()
		{
			try
			{
				var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
				var signatureHeader = Request.Headers["Stripe-Signature"];
				var webhookEndpointSecret = _config["Stripe:WebhookSecret"];

				var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, webhookEndpointSecret);
				Console.WriteLine(stripeEvent.Data.Object);

				switch (stripeEvent.Type)
				{
					case Events.CheckoutSessionCompleted:
						var session = stripeEvent.Data.Object as Session;
						return await HandleSessionSuccess(session!.Id);

					default:
						break;
				}
			}
			catch (CommandeException e)
			{
				return BadRequest(new { e.Cause, e.Message });
			}
			catch (Exception e)
			{
				return BadRequest();
			}

			return Ok();
		}

		[NonAction]
		public async Task<IActionResult> HandleSessionSuccess(string sessionId)
		{
			var service = new SessionService();
			var session = await service.GetAsync(sessionId, new()
			{
				Expand = new()
				{
					"customer",
					"line_items.data.price.product",
					"invoice",
					"shipping_cost.shipping_rate"
				}
			});

			int iduser = int.Parse(session.ClientReferenceId);
			Utilisateur? user = await _context.Utilisateurs.FindAsync(iduser);
			if (user is null) return Unauthorized();

			Commande commande;
			try { commande = await RegisterStripeCommand(session, user); }
			catch (CommandeException e)
			{
				return StatusCode(500, new { e.Cause, e.Message });
			}

			return Ok();
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
				RueFacturation = adrFacturation.Line1
			};

			List<Tuple<StockProduit, int>> quantityToRemove = new();

			foreach (var item in session.LineItems.Data)
			{
				int idVCProduit = int.Parse(item.Price.Product.Metadata["IdVCProduit"]);
				int idTaille = int.Parse(item.Price.Product.Metadata["IdTaille"]);

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

				quantityToRemove.Add(new(stocks, (int)item.Quantity));
			}

			commande.Status.Add(new()
			{
				Commande = commande,
				Code = CodeStatusCommande.Preparation
			});

			await _context.Commandes.AddAsync(commande);

			// Remove from stocks later and all at once to prevent unwanted updates (if an exception occurs early)
			foreach (var stocksQuantity in quantityToRemove)
				stocksQuantity.Item1.Stocks -= stocksQuantity.Item2;

			await _context.SaveChangesAsync();

			return commande;
		}
	}
}

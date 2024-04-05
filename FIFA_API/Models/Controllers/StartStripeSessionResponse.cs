namespace FIFA_API.Models.Controllers
{
    /// <summary>
    /// Réponse de début de session Stripe.
    /// </summary>
    public class StartStripeSessionResponse
    {
        /// <summary>
        /// Lien vers la session de paiement.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// La clé publique du compte Stripe.
        /// </summary>
        public string PublicKey { get; set; }
    }
}

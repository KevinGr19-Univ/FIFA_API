namespace FIFA_API.Contracts
{
    /// <summary>
    /// Interface utilisée pour envoyer des SMS.
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// Envoie un SMS.
        /// </summary>
        /// <param name="to">Le numéro de téléphone du destinaire.</param>
        /// <param name="message">Le message à envoyer.</param>
        Task SendSMSAsync(string to, string message);
    }
}

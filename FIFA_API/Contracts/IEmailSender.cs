namespace FIFA_API.Contracts
{
    /// <summary>
    /// Interface utilisée pour envoyer desmails.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Envoie un mail.
        /// </summary>
        /// <param name="to">L'adresse mail du destinataire.</param>
        /// <param name="subject">Le sujet du mail.</param>
        /// <param name="message">Le message du mail.</param>
        Task SendAsync(string to, string subject, string message);
    }
}

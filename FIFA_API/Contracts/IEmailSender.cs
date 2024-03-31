namespace FIFA_API.Contracts
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string message);
    }
}

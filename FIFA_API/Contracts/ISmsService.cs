namespace FIFA_API.Contracts
{
    public interface ISmsService
    {
        Task SendSMSAsync(string to, string message);
    }
}

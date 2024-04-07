namespace FIFA_API.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        Task SaveChanges();
    }
}

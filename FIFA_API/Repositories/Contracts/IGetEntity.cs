namespace FIFA_API.Repositories
{
    public interface IGetEntity<TKey, TEntity> where TEntity : class
    {
        Task<TEntity?> GetById(TKey id);
        Task<bool> Exists(TKey id);
    }

    //public interface IGetEntity<TKey1, TKey2, TEntity> where TEntity : class
    //{
    //    Task<TEntity?> GetById(TKey1 key1, TKey2 key2);
    //    Task<bool> Exists(TKey1 key1, TKey2 key2);
    //}
}

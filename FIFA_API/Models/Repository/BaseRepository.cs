using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FIFA_API.Models.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        #region Reflection
        private static readonly Dictionary<Type, PropertyInfo> DBSET_CACHE;
        private static readonly Dictionary<Type, PropertyInfo[]> ENTITY_PROPERTIES_CACHE;

        static BaseRepository()
        {
            DBSET_CACHE = new Dictionary<Type, PropertyInfo>();
            ENTITY_PROPERTIES_CACHE = new Dictionary<Type, PropertyInfo[]>();

            CacheDbSetProperties();
        }

        public static void CacheDbSetProperties()
        {
            Type dbType = typeof(FifaDbContext);

            foreach(PropertyInfo propInfo in dbType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                Type propType = propInfo.PropertyType;
                if (!(propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(DbSet<>))) continue;

                Type dbSetType = propType.GenericTypeArguments[0];
                DBSET_CACHE.Add(dbSetType, propInfo);
                ENTITY_PROPERTIES_CACHE.Add(dbSetType, dbSetType.GetProperties(BindingFlags.Public | BindingFlags.Instance 
                    | BindingFlags.GetProperty | BindingFlags.SetProperty));

            }
        }

        private static Func<DbSet<U>>? GetDbSetProperty<U>(FifaDbContext dbContext) where U : class
        {
            if (!DBSET_CACHE.TryGetValue(typeof(U), out PropertyInfo? prop)) return null;
            return () => (DbSet<U>)prop.GetValue(dbContext)!;
        }
        #endregion

        public DbSet<T> DbSet => _getDbSet();

        private readonly Func<DbSet<T>> _getDbSet;
        protected readonly FifaDbContext dbContext;

        public BaseRepository(FifaDbContext dbContext)
        {
            _getDbSet = GetDbSetProperty<T>(dbContext);
            if (_getDbSet == null)
                throw new ArgumentException($"Le type {dbContext.GetType().Name} n'a pas de propriété de type {nameof(DbSet<T>)}");

            this.dbContext = dbContext;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task AddAsync(T elementToAdd)
        {
            await DbSet.AddAsync(elementToAdd);
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T elementToUpdate, T sourceElement)
        {
            dbContext.Entry(elementToUpdate).State = EntityState.Modified;

            foreach(PropertyInfo propInfo in ENTITY_PROPERTIES_CACHE[typeof(T)])
                propInfo.SetValue(elementToUpdate, propInfo.GetValue(sourceElement));

            await dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T elementToDelete)
        {
            DbSet.Remove(elementToDelete);
            await dbContext.SaveChangesAsync();
        }
        
    }
}

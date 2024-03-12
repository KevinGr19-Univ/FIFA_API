using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FIFA_API.Models.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        #region Reflection
        private static readonly Dictionary<Type, Dictionary<Type, PropertyInfo>> DBSET_CACHE;
        private static readonly Dictionary<Type, PropertyInfo[]> ENTITY_PROPERTIES_CACHE;

        static BaseRepository()
        {
            DBSET_CACHE = new Dictionary<Type, Dictionary<Type, PropertyInfo>>();
            ENTITY_PROPERTIES_CACHE = new Dictionary<Type, PropertyInfo[]>();
        }

        public static void CacheDbSetProperties(DbContext dbContext, bool force = false)
        {
            Type dbType = dbContext.GetType();
            if (!force && DBSET_CACHE.ContainsKey(dbType)) return;

            Dictionary<Type, PropertyInfo> dbSetProps = new();
            foreach(PropertyInfo propInfo in dbType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                Type propType = propInfo.PropertyType;
                if (force && DBSET_CACHE[dbType].ContainsKey(propType))
                {
                    // TODO: Warn about duplicate DbSet's
                }
                if (!(propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(DbSet<>))) continue;

                dbSetProps.Add(propType.GenericTypeArguments[0], propInfo);
            }

            DBSET_CACHE.Add(dbType, dbSetProps);
        }

        private static Func<DbSet<U>>? GetDbSetProperty<U>(DbContext dbContext) where U : class
        {
            if (!DBSET_CACHE.TryGetValue(dbContext.GetType(), out Dictionary<Type, PropertyInfo>? dbSetProps)
                || !dbSetProps.TryGetValue(typeof(U), out PropertyInfo? propInfo)) return null;

            return (() => (DbSet<U>)propInfo.GetValue(dbContext)!);
        }

        public static void CacheEntityProperties<U>(bool force = false)
        {
            Type entityType = typeof(U);
            if (!force && ENTITY_PROPERTIES_CACHE.ContainsKey(entityType)) return;

            ENTITY_PROPERTIES_CACHE.Add(entityType, entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty)
                //.Where(p => p.GetCustomAttribute<Column>() != null)
            );
        }
        #endregion

        public DbSet<T> DbSet => _getDbSet();

        private readonly Func<DbSet<T>> _getDbSet;
        protected readonly DbContext dbContext;

        public BaseRepository(DbContext dbContext)
        {
            CacheDbSetProperties(dbContext);
            _getDbSet = GetDbSetProperty<T>(dbContext);

            if (_getDbSet == null)
                throw new ArgumentException($"Le type {dbContext.GetType().Name} n'a pas de propriété de type {nameof(DbSet<T>)}");

            this.dbContext = dbContext;
        }

        public async Task<ActionResult<IEnumerable<T>>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task AddAsync(T elementToAdd)
        {
            await DbSet.AddAsync(elementToAdd);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T elementToUpdate, T sourceElement)
        {
            dbContext.Entry(elementToUpdate).State = EntityState.Modified;

            foreach(PropertyInfo propInfo in ENTITY_PROPERTIES_CACHE[typeof(T)])
                propInfo.SetValue(elementToUpdate, propInfo.GetValue(sourceElement));

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T elementToDelete)
        {
            DbSet.Remove(elementToDelete);
            await dbContext.SaveChangesAsync();
        }
        
    }
}

using FIFA_API.Models.Annotations;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FIFA_API.Models.Utils
{
    /// <summary>
    /// Utilitaires pour la création des modèles de <see cref="DbContext"/>.
    /// </summary>
    public static class DbContextUtils
    {
        #region Composed keys
        /// <summary>
        /// Utilise la réflection pour assigner des clés composés aux entités ayant l'attribut <see cref="ComposedKeyAttribute"/>.
        /// </summary>
        /// <param name="mb">Le model builder à utiliser.</param>
        public static void AddComposedPrimaryKeys(ModelBuilder mb)
        {
            foreach (var entity in mb.Model.GetEntityTypes())
            {
                if (entity.IsPropertyBag) continue;
                ComposedKeyAttribute? cKey = entity.ClrType.GetCustomAttribute<ComposedKeyAttribute>();
                if (cKey is not null) mb.Entity(entity.ClrType).HasKey(cKey.keys);
            }
        }
        #endregion

        #region Many to many
        /// <summary>
        /// Les flags utilisés lors de la recherche des propriétés ayant l'attribut <see cref="ManyToManyAttribute"/>.
        /// </summary>
        private const BindingFlags MTM_PROP_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Utilise la réflection pour lier deux propriétés ayant l'attribut <see cref="ManyToManyAttribute"/>.
        /// </summary>
        /// <remarks>NOTE: Gère aussi les attributs <see cref="OnDeleteAttribute"/>.</remarks>
        /// <param name="mb">Le model builder à utiliser.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void AddManyToManyRelations(ModelBuilder mb)
        {
            HashSet<PropertyInfo> cache = new HashSet<PropertyInfo>();

            foreach (var entity in mb.Model.GetEntityTypes().ToList())
            {
                if (entity.IsPropertyBag) continue;
                foreach (var prop in entity.ClrType.GetProperties(MTM_PROP_FLAGS))
                {
                    if (cache.Contains(prop)) continue;

                    ManyToManyAttribute? propMTM = prop.GetCustomAttribute<ManyToManyAttribute>();
                    if (propMTM is null) continue;

                    Type typeU = GetCollectionType(prop);
                    cache.Add(prop);

                    PropertyInfo? inverseProperty = typeU.GetProperty(propMTM.inverseProperty, MTM_PROP_FLAGS);
                    if (inverseProperty is null || cache.Contains(inverseProperty))
                        throw new ArgumentException($"L'inverse property de {nameof(ManyToManyAttribute)} n'existe pas : {typeU.Name}.{propMTM.inverseProperty}");

                    Type typeT = GetCollectionType(inverseProperty);
                    if (typeT != entity.ClrType)
                        throw new ArgumentException($"L'inverse property de {nameof(ManyToManyAttribute)} n'est pas de type {entity.ClrType}");

                    cache.Add(inverseProperty);

                    DeleteBehavior? deleteT = prop.GetCustomAttribute<OnDeleteAttribute>()?.deleteBehavior;
                    DeleteBehavior? deleteU = inverseProperty.GetCustomAttribute<OnDeleteAttribute>()?.deleteBehavior;

                    ManyToMany(mb, typeT, prop, deleteT, typeU, inverseProperty, deleteU, propMTM.joinTableName);
                }
            }
        }

        private static Type GetCollectionType(PropertyInfo prop)
        {
            Type propType = prop.PropertyType;
            if (!(propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(ICollection<>)))
                throw new ArgumentException($"{nameof(ManyToManyAttribute)} ne s'applique que sur des collections");

            return propType.GetGenericArguments()[0];
        }

        private static void ManyToMany(ModelBuilder mb, Type typeT, PropertyInfo propT, DeleteBehavior? deleteT, Type typeU, PropertyInfo propU, DeleteBehavior? deleteU, string? joinTableName)
        {
            if (!typeT.IsClass || !typeU.IsClass)
                throw new ArgumentException("Les types donnés doivent être des entités");

            deleteT ??= DEFAULT_DELETE_MANY_TO_MANY;
            deleteU ??= DEFAULT_DELETE_MANY_TO_MANY;
            joinTableName ??= $"t_j_{typeT.Name}{typeU.Name}_{typeT.Name[..2]}{typeU.Name[0]}".ToLower();

            IMutableProperty[] keysT = mb.Entity(typeT).Metadata.FindPrimaryKey()!.Properties.ToArray();
            IMutableProperty[] keysU = mb.Entity(typeU).Metadata.FindPrimaryKey()!.Properties.ToArray();

            List<string> finalKey = new List<string>();
            List<string> columnCache = new List<string>();

            void AddKeys(EntityTypeBuilder j, PropertyInfo inverseProp, IMutableProperty[] keys)
            {
                foreach (var key in keys)
                {
                    string name = $"{inverseProp.Name}{key.Name}";
                    finalKey.Add(name);

                    string columnName = key.GetColumnBaseName();

                    if (columnCache.Contains(columnName)) columnName += "_bis";
                    columnCache.Add(columnName);

                    j.Property(key.ClrType, name).HasColumnName(columnName);
                }
            }

            mb.Entity(typeT, entity =>
            {
                entity.HasMany(typeU, propT.Name)
                    .WithMany(propU.Name)
                    .UsingEntity(joinTableName, j =>
                    {
                        AddKeys(j, propU, keysT);
                        AddKeys(j, propT, keysU);
                        j.HasKey(finalKey.ToArray());
                    });

                foreach (var fk in entity.Metadata.GetDeclaredForeignKeys())
                {
                    if (fk.PrincipalKey.Properties.SequenceEqual(keysT)) fk.DeleteBehavior = (DeleteBehavior)deleteT;
                    else if (fk.PrincipalKey.Properties.SequenceEqual(keysU)) fk.DeleteBehavior = (DeleteBehavior)deleteU;
                }
            });
        }
        #endregion

        #region OnDelete
        /// <summary>
        /// Le <see cref="DeleteBehavior"/> par défaut.
        /// </summary>
        public const DeleteBehavior DEFAULT_DELETE = DeleteBehavior.Restrict;

        /// <summary>
        /// Le <see cref="DeleteBehavior"/> par défaut dans les relations many-to-many.
        /// </summary>
        public const DeleteBehavior DEFAULT_DELETE_MANY_TO_MANY = DeleteBehavior.Cascade;

        /// <summary>
        /// Utilise la réflection pour changer le <see cref="DeleteBehavior"/> des clés étrangères ayant l'attribut <see cref="OnDeleteAttribute"/>.
        /// </summary>
        /// <remarks>NOTE: Cette méthode gère les relations one-to-one et one-to-many. 
        /// Voir <see cref="AddManyToManyRelations(ModelBuilder)"/> pour les relations many-to-many.</remarks>
        /// <param name="mb">Le model builder à utiliser.</param>
        public static void AddDeleteBehaviors(ModelBuilder mb)
        {
            foreach (var fk in mb.Model.GetEntityTypes().SelectMany(e => e.GetDeclaredForeignKeys()))
            {
                if (fk.DeclaringEntityType.IsPropertyBag) continue;

                DeleteBehavior? deleteBehavior = fk.GetNavigations()
                    .Select(n => n.PropertyInfo)
                    .Select(p => p.GetCustomAttribute<OnDeleteAttribute>())
                    .Where(a => a != null)
                    .FirstOrDefault()?.deleteBehavior;

                fk.DeleteBehavior = deleteBehavior ?? DEFAULT_DELETE;
            }
        }
        #endregion

        #region Constraint renaming
        /// <summary>
        /// Le regex utilisé pour détecter les tables avec la bonne convention de nommage.
        /// </summary>
        public const string TABLE_CONVENTION_REGEX = "^t_[a-z]_[a-z]+_[a-z]{3}$";

        /// <summary>
        /// Renomme chaque clé étrangère et index de chaque entité.
        /// </summary>
        /// <remarks>NOTE: Ne marche seulement avec les tables suivant la norme ISO 9075.
        /// Voir <see cref="TABLE_CONVENTION_REGEX"/>.</remarks>
        /// <param name="mb">Le model builder à utiliser.</param>
        public static void RenameConstraintsAuto(ModelBuilder mb)
        {
            foreach (var entity in mb.Model.GetEntityTypes())
            {
                string tableName = entity.GetTableName()!;
                if (!new Regex(TABLE_CONVENTION_REGEX).IsMatch(tableName)) continue;

                tableName = tableName.Split("_")[2]; // t_e_photo_pht -> photo

                foreach (var fk in entity.GetDeclaredForeignKeys())
                {
                    string fkName = GetConstraintName("FK", tableName, fk.Properties);
                    fk.SetConstraintName(fkName);
                }

                foreach (var ix in entity.GetIndexes())
                {
                    string ixName = GetConstraintName("IX", tableName, ix.Properties);
                    ix.SetDatabaseName(ixName);
                }
            }
        }

        private static string GetConstraintName(string prefix, string tableName, IReadOnlyList<IMutableProperty> properties)
            => $"{prefix}_{tableName}_{string.Join("_", properties.Select(p => p.GetColumnBaseName()))}";
        #endregion
    }
}

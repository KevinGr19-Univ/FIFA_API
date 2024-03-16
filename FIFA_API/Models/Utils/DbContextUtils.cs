using FIFA_API.Models.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FIFA_API.Models.Utils
{
    public static class DbContextUtils
    {
        #region Many to many
        private const BindingFlags MTM_PROP_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

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
                    if (propMTM == null) continue;

                    Type typeU = GetCollectionType(prop);
                    cache.Add(prop);

                    PropertyInfo? inverseProperty = typeU.GetProperty(propMTM.inverseProperty, MTM_PROP_FLAGS);
                    if (inverseProperty == null || cache.Contains(inverseProperty))
                        throw new ArgumentException($"L'inverse property de {nameof(ManyToManyAttribute)} n'existe pas : {typeU.Name}.{propMTM.inverseProperty}");

                    Type typeT = GetCollectionType(inverseProperty);
                    if (typeT != entity.ClrType)
                        throw new ArgumentException($"L'inverse property de {nameof(ManyToManyAttribute)} n'est pas de type {entity.ClrType}");

                    cache.Add(inverseProperty);
                    ManyToMany(mb, typeT, prop, typeU, inverseProperty, propMTM.joinTableName);
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

        private static void ManyToMany(ModelBuilder mb, Type typeT, PropertyInfo propT, Type typeU, PropertyInfo propU, string? joinTableName)
        {
            if (!typeT.IsClass || !typeU.IsClass)
                throw new ArgumentException("Les types donnés doivent être des entités");

            joinTableName ??= $"t_j_{typeT.Name}{typeU.Name}_{typeT.Name[..2]}{typeU.Name[2]}".ToLower();

            IMutableProperty[] keysT = mb.Entity(typeT).Metadata.FindPrimaryKey()!.Properties.ToArray();
            IMutableProperty[] keysU = mb.Entity(typeU).Metadata.FindPrimaryKey()!.Properties.ToArray();

            mb.Entity(typeT, entity =>
            {
                entity.HasMany(typeU, propT.Name)
                    .WithMany(propU.Name)
                    .UsingEntity(joinTableName, j =>
                    {
                        List<string> finalKey = new List<string>();

                        foreach (var key in keysT)
                        {
                            string name = $"{propU.Name}{key.Name}";
                            finalKey.Add(name);
                            j.Property(key.ClrType, name).HasColumnName(key.GetColumnBaseName());
                        }

                        foreach (var key in keysU)
                        {
                            string name = $"{propT.Name}{key.Name}";
                            finalKey.Add(name);
                            j.Property(key.ClrType, name).HasColumnName(key.GetColumnBaseName());
                        }

                        j.HasKey(finalKey.ToArray());
                    });
            });
        }
        #endregion

        #region Constraint renaming
        public const string TABLE_CONVENTION_REGEX = "^t_[a-z]_[a-z]+_[a-z]{3}$";

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

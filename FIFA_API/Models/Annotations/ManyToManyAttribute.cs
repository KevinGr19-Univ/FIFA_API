namespace FIFA_API.Models.Annotations
{
    /// <summary>
    /// Links two properties as a Many-to-Many relationship.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ManyToManyAttribute : Attribute
    {
        public readonly string inverseProperty;
        public readonly string? joinTableName;

        public ManyToManyAttribute(string inverseProperty, string? joinTableName = null)
        {
            this.inverseProperty = inverseProperty;
            this.joinTableName = joinTableName;
        }
    }
}

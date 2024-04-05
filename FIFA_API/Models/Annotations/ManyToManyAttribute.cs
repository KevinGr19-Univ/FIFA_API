namespace FIFA_API.Models.Annotations
{
    /// <summary>
    /// Relie deux propriétés par une relation many-to-many.
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

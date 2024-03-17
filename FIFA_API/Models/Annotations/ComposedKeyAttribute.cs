namespace FIFA_API.Models.Annotations
{
    /// <summary>
    /// Assigns multiple properties as primary key of the table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ComposedKeyAttribute : Attribute
    {
        public readonly string[] keys;

        public ComposedKeyAttribute(params string[] keys)
        {
            this.keys = keys;
        }
    }
}

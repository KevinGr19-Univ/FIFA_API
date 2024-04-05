namespace FIFA_API.Models.Annotations
{
    /// <summary>
    /// Assigne une clé primaire composée à la classe.
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

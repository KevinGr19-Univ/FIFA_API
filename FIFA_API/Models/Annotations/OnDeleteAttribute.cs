using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Annotations
{
    /// <summary>
    /// Change le <see cref="DeleteBehavior"/> de la clé étrangère associée à la propriété.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OnDeleteAttribute : Attribute
    {
        public readonly DeleteBehavior deleteBehavior;

        public OnDeleteAttribute(DeleteBehavior deleteBehavior)
        {
            this.deleteBehavior = deleteBehavior;
        }
    }
}

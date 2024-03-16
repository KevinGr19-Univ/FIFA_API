using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Annotations
{
    /// <summary>
    /// Sets the <see cref="DeleteBehavior"/> of the foreign key associated with the property.
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

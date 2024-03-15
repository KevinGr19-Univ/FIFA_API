using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Annotations
{
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

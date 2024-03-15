using Microsoft.EntityFrameworkCore;

namespace FIFA_API.Models.Utils
{
    public class ManyToManyProperty<T>
    {
        public readonly string TypeName = typeof(T).Name.ToLower();

        public string PropertyName { get; set; }
        public string ColumnName { get; set; }
        public DeleteBehavior? DeleteBehavior { get; set; }
        public Type? KeyType { get; set; }
    }
}

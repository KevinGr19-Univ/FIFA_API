namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class Album : Publication
    {
        public ICollection<Photo> Photos { get; set; }
    }
}

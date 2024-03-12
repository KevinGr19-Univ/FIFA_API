namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class Blog : Publication
    {
        public string TexteBlog { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}

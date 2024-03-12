namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public abstract class Publication
    {
        public string Titre { get; set; }
        public string Resume { get; set; }
        public DateTime DatePublication { get; set; }
        public Photo? Photo { get; set; }
    }
}

namespace FIFA_API.Models.EntityFramework
{
    public abstract class Publication
    {
        public string Titre { get; set; }
        public string Resume { get; set; }
        public DateTime DatePublication { get; set; }
        public Photo? Photo { get; set; }
    }
}

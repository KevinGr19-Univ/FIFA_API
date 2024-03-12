namespace FIFA_API.Models.LE_CODE_FIRST____
{
    public class Trophee
    {
        public string Nom { get; set; }

        public ICollection<Joueur> Joueurs { get; set; }
    }
}

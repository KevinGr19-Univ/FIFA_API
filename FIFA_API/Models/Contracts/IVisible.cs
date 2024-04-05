namespace FIFA_API.Models.Contracts
{
    /// <summary>
    /// Interface de modèle pour activer et désactiver la visibilité de l'entité.
    /// </summary>
    public interface IVisible
    {
        int Id { get; }
        bool Visible { get; }
    }
}

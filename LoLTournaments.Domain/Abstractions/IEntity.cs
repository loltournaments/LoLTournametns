namespace LoLTournaments.Domain.Abstractions
{
    public interface IEntity
    {
        string Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime OnUpdated { get; set; }
    }
}
namespace LoLTournaments.Domain.Abstractions
{
    public abstract class BaseEntity : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime OnUpdated { get; set; } = DateTime.UtcNow;
    }
}
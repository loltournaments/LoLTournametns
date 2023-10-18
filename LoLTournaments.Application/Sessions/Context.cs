using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Application.Sessions
{

    public abstract class Context<T> where T : IIdentity
    {
        protected readonly SynchronizedCollection<T> ContextItems = new();

        public virtual List<T> GetItems()
        {
            ContextItems.SortIfOrderable();
            return ContextItems.ToList();
        }

        public virtual void Reset()
        {
            ContextItems.Clear();
        }
    }
}
using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Application.Sessions
{

    public abstract class Context<T> where T : IIdentity
    {
        protected readonly SynchronizedCollection<T> ContextItems = new();

        public T Current
        {
            get
            {
                if (requestRefreshCurrent && IsValidCurrent())
                    SelectCurrent(current.Id);
                return current;
            }
        }

        private T current;
        private bool requestRefreshCurrent;
        
        public virtual List<T> GetItems()
        {
            ContextItems.SortIfOrderable();
            return ContextItems.ToList();
        }

        public virtual void Reset()
        {
            ContextItems.Clear();
            current = default;
        }

        public Result SelectCurrent(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                current = default;
                return Result.Failure();
            }
            
            current = ContextItems.FirstOrDefault(x => x.Id == id);
            requestRefreshCurrent = false;
            return IsValidCurrent() ? Result.Success() : Result.Failure();
        }

        public bool IsValidCurrent()
        {
            return !string.IsNullOrEmpty(Current?.Id);
        }

        protected void RequestToRefreshCurrent()
        {
            requestRefreshCurrent = true;
        }
    }
}
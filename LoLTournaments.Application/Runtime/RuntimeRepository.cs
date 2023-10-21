using LoLTournaments.Application.Abstractions;
using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Application.Runtime
{

    public class RuntimeRepository<TValue> : IRuntimeRepository<TValue> where TValue : IIdentity
    {
        protected readonly SynchronizedCollection<TValue> Storage;

        public RuntimeRepository()
        {
            Storage = new SynchronizedCollection<TValue>();
        }

        public virtual TValue Get(string key)
        {
            return Storage.FirstOrDefault(x => x.Id == key);
        }

        public TValue[] Get(Func<TValue, bool> predicat)
        {
            return Storage.Where(predicat).ToArray();
        }

        public virtual TValue[] Get()
        {
            return Storage.ToArray();
        }

        public virtual void Add(TValue value)
        {
            if (Storage.Contains(value))
                return;

            Storage.Add(value);
        }

        public bool Replace(TValue value)
        {
            return Storage.Replace(value);
        }

        public bool Replace(IEnumerable<TValue> value)
        {
            return Storage.Replace(value);
        }

        public virtual bool Remove(TValue value)
        {
            return Storage.Remove(value);
        }

        public virtual bool Remove(string key)
        {
            var toRemove = Storage.Where(x => x.Id == key).ToArray();
            if (!toRemove.Any())
                return false;

            toRemove.Foreach(obj => Remove(obj));
            return true;
        }

        public virtual void Clear()
        {
            Storage.Clear();
        }
    }

}
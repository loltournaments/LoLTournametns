using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Domain.Abstractions
{

    public interface IRuntimeRepository<TValue> where TValue : IIdentity
    {
        TValue Get(string key);
        IEnumerable<TValue> Get(Func<TValue, bool> predicat);
        IEnumerable<TValue> Get();
        void Add(TValue value);
        void AddRange(IEnumerable<TValue> value);
        bool Replace(TValue value);
        bool Replace(IEnumerable<TValue> value);
        bool Remove(TValue value);
        bool Remove(string key);
        void Clear();
    }

}
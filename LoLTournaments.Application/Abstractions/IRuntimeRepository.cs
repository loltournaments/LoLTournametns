using LoLTournaments.Shared.Abstractions;

namespace LoLTournaments.Application.Abstractions
{

    public interface IRuntimeRepository<TValue> where TValue : IIdentity
    {
        TValue Get(string key);
        TValue[] Get(Func<TValue, bool> predicat);
        TValue[] Get();
        void Add(TValue value);
        bool Replace(TValue value);
        bool Replace(IEnumerable<TValue> value);
        bool Remove(TValue value);
        bool Remove(string key);
        void Clear();
    }

}
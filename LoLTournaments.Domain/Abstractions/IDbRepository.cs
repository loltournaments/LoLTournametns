using System.Linq.Expressions;

namespace LoLTournaments.Domain.Abstractions
{

    public interface IDbRepository
    {
        IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IEntity;
        IQueryable<T> Get<T>() where T : class, IEntity;

        Task AddAsync<T>(T entity) where T : class, IEntity;
        Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class, IEntity;

        Task RemoveAsync<T>(T entity) where T : class, IEntity;
        Task RemoveAsync<T>(string id) where T : class, IEntity;
        Task RemoveRangeAsync<T>(IEnumerable<T> entities) where T : class, IEntity;

        Task UpdateAsync<T>(T entity) where T : class, IEntity;
        Task UpdateRange<T>(IEnumerable<T> entities) where T : class, IEntity;

        Task SaveChangesAsync();
    }

}
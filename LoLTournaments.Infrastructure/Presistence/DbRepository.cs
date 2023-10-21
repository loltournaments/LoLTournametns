using System.Linq.Expressions;
using LoLTournaments.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LoLTournaments.Infrastructure.Presistence
{

    public class DbRepository<TContext> : IDbRepository where TContext : DbContext
    {
        private readonly TContext context;

        public DbRepository(TContext context)
        {
            this.context = context;
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> selector) where T : class, IEntity
        {
            return context.Set<T>().Where(selector).AsQueryable();
        }

        public IQueryable<T> Get<T>() where T : class, IEntity
        {
            return context.Set<T>().AsQueryable();
        }

        public async Task AddAsync<T>(T entity) where T : class, IEntity
        {
            await context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class, IEntity
        {
            await context.Set<T>().AddRangeAsync(entities);
        }

        public Task RemoveAsync<T>(T entity) where T : class, IEntity
        {
            context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task RemoveAsync<T>(string id) where T : class, IEntity
        {
            var entity = await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            await RemoveAsync(entity);
        }

        public Task RemoveRangeAsync<T>(IEnumerable<T> entities) where T : class, IEntity
        {
            context.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task UpdateAsync<T>(T entity) where T : class, IEntity
        {
            context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateRange<T>(IEnumerable<T> entities) where T : class, IEntity
        {
            context.Set<T>().UpdateRange(entities);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }

}
using AutoMapper;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LoLTournaments.Application.Services
{

    public interface IRuntimeBackupService<TData> where TData : IIdentity
    {
        Task BackupAsync(bool force = false);
        Task RestoreAsync(bool force = false);
    }
    
    public class BackupService<TEntity, TData>: IRuntimeBackupService<TData> 
        where TEntity : class,IEntity where TData: IIdentity
    {
        private readonly IMapper mapper;
        private readonly ISharedTime sharedTime;
        private readonly IDbRepository dbRepository;
        private readonly IRuntimeRepository<TData> runtimeRepository;
        private const double TimeBetweenBackup = 20d;
        private DateTime nextBackup;

        public BackupService(
            IMapper mapper,
            ISharedTime sharedTime,
            IDbRepository dbRepository,
            IRuntimeRepository<TData> runtimeRepository)
        {
            this.mapper = mapper;
            this.sharedTime = sharedTime;
            this.dbRepository = dbRepository;
            this.runtimeRepository = runtimeRepository;
        }

        public async Task BackupAsync(bool force = false)
        {
            if (!force && nextBackup > sharedTime.Current)
                return;
            
            nextBackup = sharedTime.Current + TimeSpan.FromSeconds(TimeBetweenBackup);
            var newEntities = mapper.Map<TEntity[]>(runtimeRepository.Get());
            var oldEntities = await dbRepository.Get<TEntity>().ToListAsync();
            
            if (oldEntities.Any())
                await dbRepository.RemoveRangeAsync(oldEntities);
            
            if (newEntities.Any())
                await dbRepository.AddRangeAsync(newEntities);
            
            if (newEntities.Any() || oldEntities.Any())
                await dbRepository.SaveChangesAsync();
        }

        public async Task RestoreAsync(bool force = false)
        {
            var availableEntities = await dbRepository.Get<TEntity>().ToListAsync();
            var restoredEntities = mapper.Map<TData[]>(availableEntities);
            runtimeRepository.Clear();
            runtimeRepository.AddRange(restoredEntities);
        }
    }

}
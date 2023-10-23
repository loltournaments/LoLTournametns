using LoLTournaments.Application.Exceptions;
using LoLTournaments.Application.Runtime;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Application.Services
{
    public interface ISessionService{}
    public class SessionService : ISessionService
    {
        private readonly IRuntimeRepository<RuntimeSession> runtimeRepository;
        public SessionService(IRuntimeRepository<RuntimeSession> runtimeRepository)
        {
            this.runtimeRepository = runtimeRepository;
        }

        public Task<dynamic> GetSessions()
        {
            return Task.FromResult<dynamic>(runtimeRepository.Get().ToList().SortIfOrderable());
        }

        public Task<dynamic> GetSession(RequestSession model)
        {
            return Task.FromResult<dynamic>(RequestSession(model));
        }

        public Task<dynamic> GetSessionData(RequestSessionData model)
        {
            var session = RequestSession(model);
            // ReSharper disable once HeapView.BoxingAllocation
            return Task.FromResult<dynamic>(model.PropertyName switch
            {
                nameof(session.Members) => session.Members.SortIfOrderable(),
                nameof(session.Order) => session.Order,
                nameof(session.Stages) => session.Stages.SortIfOrderable(),
                nameof(session.Step) => session.Step,
                nameof(session.Winners) => session.Winners.SortIfOrderable(),
                nameof(session.Id) => session.Id,
                nameof(session.Version) => session.Version,
                _ => throw new ClientException(
                    $"Get session data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]")
            });
        }
        
        public Task SetSessionData(ReceiveSessionData model)
        {
            var session = RequestSession(model);
            
            switch (model.PropertyName)
            {
                case nameof(session.Members):
                {
                    session.Members.Clear();
                    model.Data.GetValue<RuntimeMember[]>().SortIfOrderable().Foreach(session.Members.Add);
                    break;
                }
                case nameof(session.Stages):
                {
                    session.Stages.Clear();
                    model.Data.GetValue<RuntimeStage[]>().SortIfOrderable().Foreach(session.Stages.Add);
                    break;
                }
                case nameof(session.Winners):                 
                {
                    session.Winners.Clear();
                    model.Data.GetValue<RuntimeWinner[]>().SortIfOrderable().Foreach(session.Winners.Add);
                    break;
                }
                case nameof(session.Order): session.Order = model.Data.GetValue(session.Order); break;
                case nameof(session.Step): session.Step  = model.Data.GetValue(session.Step); break;
                case nameof(session.Id): session.Id = model.Data.GetValue(session.Id); break;
                case nameof(session.Version): session.Version = model.Data.GetValue(session.Version); break;
                default: throw new ClientException(
                        $"Set session data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]");
            }
            
            return Task.CompletedTask;
        }

        public Task UpdateSessions(ReceiveSessionData model)
        {
            if (!model.Data.TryGetValue(out RuntimeSession[] sessions))
                throw new ClientException($"Can't update sessions, sessions is missing : {model}.");

            runtimeRepository.Replace(sessions);
            return Task.CompletedTask;
        }
        
        public Task RemoveSession(RequestSession model)
        {
            runtimeRepository.Remove(model.SessionId);
            return Task.CompletedTask;
        }
        
        private RuntimeSession RequestSession(RequestSession model)
        {
            var room = runtimeRepository.Get(model.SessionId);
            if (room == null)
                throw new ClientException($"Session with id : {model.SessionId} doesn't exist.\n" +
                                          $"Get Request : {model}");
            return room;
        }
    }

}
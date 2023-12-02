using System.Dynamic;
using LoLTournaments.Application.Exceptions;
using LoLTournaments.Application.Runtime;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Application.Services
{

    public interface ISessionService
    {
        Task<dynamic> GetSessions();
        Task<dynamic> GetSession(RequestSession model);
        Task<dynamic> GetStage(RequestStage model);
        Task<dynamic> GetGroup(RequestGroup model);
        Task<dynamic> GetGame(RequestGame model);
        Task<dynamic> GetSessionData(RequestSessionData model);
        Task<dynamic> GetStageData(RequestStageData model);
        Task<dynamic> GetGroupData(RequestGroupData model);
        Task<dynamic> GetGameData(RequestGameData model);
        Task SetSessionData(ReceiveSessionData model);
        Task SetStageData(ReceiveStageData model);
        Task SetGroupData(ReceiveGroupData model);
        Task SetGameData(ReceiveGameData model);
        Task UpdateSession(ReceiveSessionData model);
        Task UpdateStage(ReceiveStageData model);
        Task UpdateGroup(ReceiveGroupData model);
        Task UpdateGame(ReceiveGameData model);
        Task UpdateMember(ReceiveSessionData model);
        Task RemoveMember(ReceiveSessionData model);
        Task RemoveSession(RequestSession model);
        Task RemoveStage(RequestStage model);
        Task RemoveGroup(RequestGroup model);
        Task RemoveGame(RequestGame model);
    }

    public class SessionService : ISessionService
    {
        private readonly IRuntimeRepository<RuntimeSession> runtimeRepository;
        private readonly IRuntimeBackupService<RuntimeSession> runtimeBackupService;

        public SessionService(
            IRuntimeRepository<RuntimeSession> runtimeRepository,
            IRuntimeBackupService<RuntimeSession> runtimeBackupService)
        {
            this.runtimeRepository = runtimeRepository;
            this.runtimeBackupService = runtimeBackupService;
        }

        public Task<dynamic> GetSessions()
        {
            return Task.FromResult<dynamic>(runtimeRepository.Get().ToList().SortIfOrderable());
        }

        public Task<dynamic> GetSession(RequestSession model)
        {
            return Task.FromResult<dynamic>(RequestSession(model));
        }

        public Task<dynamic> GetStage(RequestStage model)
        {
            return Task.FromResult<dynamic>(RequestStage(model));
        }

        public Task<dynamic> GetGroup(RequestGroup model)
        {
            return Task.FromResult<dynamic>(RequestGroup(model));
        }

        public Task<dynamic> GetGame(RequestGame model)
        {
            return Task.FromResult<dynamic>(RequestGame(model));
        }

        public Task<dynamic> GetSessionData(RequestSessionData model)
        {
            var session = RequestSession(model);
            
            // ReSharper disable once HeapView.BoxingAllocation
            object result = model.PropertyName switch
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
            };
            
            var resultObj = new ExpandoObject();
            resultObj.TryAdd(model.PropertyName, result);
            return Task.FromResult<dynamic>(resultObj);
        }

        public Task<dynamic> GetStageData(RequestStageData model)
        {
            var stage = RequestStage(model);
            // ReSharper disable once HeapView.BoxingAllocation
            object result = model.PropertyName switch
            {
                nameof(stage.Groups) => stage.Groups.SortIfOrderable(),
                nameof(stage.Order) => stage.Order,
                nameof(stage.State) => stage.State,
                nameof(stage.Winners) => stage.Winners.SortIfOrderable(),
                nameof(stage.Id) => stage.Id,
                nameof(stage.Version) => stage.Version,
                _ => throw new ClientException(
                    $"Get stage data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]")
            };
            
            var resultObj = new ExpandoObject();
            resultObj.TryAdd(model.PropertyName, result);
            return Task.FromResult<dynamic>(resultObj);
        } // TODO not used

        public Task<dynamic> GetGroupData(RequestGroupData model)
        {
            var group = RequestGroup(model);
            // ReSharper disable once HeapView.BoxingAllocation
            object result = model.PropertyName switch
            {
                nameof(group.Games) => group.Games.SortIfOrderable(),
                nameof(group.Members) => group.Members.SortIfOrderable(),
                nameof(group.Order) => group.Order,
                nameof(group.Tag) => group.Tag,
                nameof(group.Id) => group.Id,
                nameof(group.Version) => group.Version,
                _ => throw new ClientException(
                    $"Get group data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]")
            };
            
            var resultObj = new ExpandoObject();
            resultObj.TryAdd(model.PropertyName, result);
            return Task.FromResult<dynamic>(resultObj);
        } // TODO not used

        public Task<dynamic> GetGameData(RequestGameData model)
        {
            var game = RequestGame(model);
            // ReSharper disable once HeapView.BoxingAllocation
            object result = model.PropertyName switch
            {
                nameof(game.Members) => game.Members.SortIfOrderable(),
                nameof(game.Order) => game.Order,
                nameof(game.Timer) => game.Timer,
                nameof(game.Tour) => game.Tour,
                nameof(game.WinnerId) => game.WinnerId,
                nameof(game.WinnerReason) => game.WinnerReason,
                nameof(game.Id) => game.Id,
                nameof(game.Version) => game.Version,
                _ => throw new ClientException(
                    $"Get game data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]")
            };
            
            var resultObj = new ExpandoObject();
            resultObj.TryAdd(model.PropertyName, result);
            return Task.FromResult<dynamic>(resultObj);
        } // TODO not used

        public async Task SetSessionData(ReceiveSessionData model)
        {
            var session = RequestSession(model);

            switch (model.PropertyName)
            {
                case nameof(session.Members):
                {
                    session.Members.Clear();
                    model.Data.GetValue<List<RuntimeMember>>().SortIfOrderable().Foreach(session.Members.Add);
                    break;
                }
                case nameof(session.Stages):
                {
                    session.Stages.Clear();
                    model.Data.GetValue<List<RuntimeStage>>().SortIfOrderable().Foreach(session.Stages.Add);
                    break;
                }
                case nameof(session.Winners):
                {
                    session.Winners.Clear();
                    model.Data.GetValue<List<RuntimeWinner>>().SortIfOrderable().Foreach(session.Winners.Add);
                    break;
                }
                case nameof(session.Order):
                    session.Order = model.Data.GetValue(session.Order);
                    break;
                case nameof(session.Step):
                    session.Step = model.Data.GetValue(session.Step);
                    break;
                case nameof(session.Id):
                    session.Id = model.Data.GetValue(session.Id);
                    break;
                case nameof(session.Version):
                    session.Version = model.Data.GetValue(session.Version);
                    break;
                default:
                    throw new ClientException(
                        $"Set session data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]");
            }

            await runtimeBackupService.BackupAsync(true);
        }

        public async Task SetStageData(ReceiveStageData model)
        {
            var stage = RequestStage(model);
            switch (model.PropertyName)
            {
                case nameof(stage.Groups):
                {
                    stage.Groups.Clear();
                    model.Data.GetValue<List<RuntimeGroup>>().SortIfOrderable().Foreach(stage.Groups.Add);
                    break;
                }
                case nameof(stage.Winners):
                {
                    stage.Winners.Clear();
                    model.Data.GetValue<string[]>().Foreach(stage.Winners.Add);
                    break;
                }
                case nameof(stage.Order):
                    stage.Order = model.Data.GetValue(stage.Order);
                    break;
                case nameof(stage.State):
                    stage.State = model.Data.GetValue(stage.State);
                    break;
                case nameof(stage.Id):
                    stage.Id = model.Data.GetValue(stage.Id);
                    break;
                case nameof(stage.Version):
                    stage.Version = model.Data.GetValue(stage.Version);
                    break;
                default:
                    throw new ClientException(
                        $"Set stage data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]");
            }

            await runtimeBackupService.BackupAsync(true);
        } // TODO not used

        public async Task SetGroupData(ReceiveGroupData model)
        {
            var group = RequestGroup(model);
            switch (model.PropertyName)
            {
                case nameof(group.Games):
                {
                    group.Games.Clear();
                    model.Data.GetValue<List<RuntimeGame>>().SortIfOrderable().Foreach(group.Games.Add);
                    break;
                }
                case nameof(group.Members):
                {
                    group.Members.Clear();
                    model.Data.GetValue<string[]>().Foreach(group.Members.Add);
                    break;
                }
                case nameof(group.Order):
                    group.Order = model.Data.GetValue(group.Order);
                    break;
                case nameof(group.Tag):
                    group.Tag = model.Data.GetValue(group.Tag);
                    break;
                case nameof(group.Id):
                    group.Id = model.Data.GetValue(group.Id);
                    break;
                case nameof(group.Version):
                    group.Version = model.Data.GetValue(group.Version);
                    break;
                default:
                    throw new ClientException(
                        $"Set group data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]");
            }

            await runtimeBackupService.BackupAsync(true);
        }

        public async Task SetGameData(ReceiveGameData model)
        {
            var game = RequestGame(model);
            switch (model.PropertyName)
            {
                case nameof(game.Members):
                {
                    game.Members.Clear();
                    model.Data.GetValue<string[]>().Foreach(game.Members.Add);
                    break;
                }
                case nameof(game.Order):
                    game.Order = model.Data.GetValue(game.Order);
                    break;
                case nameof(game.Timer):
                    game.Timer = model.Data.GetValue(game.Timer);
                    break;
                case nameof(game.Tour):
                    game.Tour = model.Data.GetValue(game.Tour);
                    break;
                case nameof(game.WinnerId):
                    game.WinnerId = model.Data.GetValue(game.WinnerId);
                    break;
                case nameof(game.WinnerReason):
                    game.WinnerReason = model.Data.GetValue(game.WinnerReason);
                    break;
                case nameof(game.Id):
                    game.Id = model.Data.GetValue(game.Id);
                    break;
                case nameof(game.Version):
                    game.Version = model.Data.GetValue(game.Version);
                    break;
                default:
                    throw new ClientException(
                        $"Set game data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]");
            }

            await runtimeBackupService.BackupAsync();
        }

        public async Task UpdateSession(ReceiveSessionData model)
        {
            if (!model.Data.TryGetValue(out RuntimeSession[] sessions))
                throw new ClientException($"Can't update sessions, sessions is missing : {model}.");

            runtimeRepository.Replace(sessions);
            await runtimeBackupService.BackupAsync(true);
        }
        
        public async Task UpdateStage(ReceiveStageData model)
        {
            if (!model.Data.TryGetValue(out RuntimeStage[] stages))
                throw new ClientException($"Can't update stage, stages is missing : {model}.");

            var session = RequestSession(model);
            session.Stages.Replace(stages);
            session.Stages.SortIfOrderable();
            await runtimeBackupService.BackupAsync(true);
        }
        
        public async Task UpdateGroup(ReceiveGroupData model)
        {
            if (!model.Data.TryGetValue(out RuntimeGroup[] groups))
                throw new ClientException($"Can't update group, groups is missing : {model}.");

            var stage = RequestStage(model);
            stage.Groups.Replace(groups);
            stage.Groups.SortIfOrderable();
            await runtimeBackupService.BackupAsync(true);
        }
        
        public async Task UpdateGame(ReceiveGameData model)
        {
            if (!model.Data.TryGetValue(out RuntimeGame[] games))
                throw new ClientException($"Can't update game, games is missing : {model}.");

            var group = RequestGroup(model);
            group.Games.Replace(games);
            group.Games.SortIfOrderable();
            await runtimeBackupService.BackupAsync();
        }
        
        public async Task UpdateMember(ReceiveSessionData model)
        {
            if (!model.Data.TryGetValue(out RuntimeMember[] members))
                throw new ClientException($"Can't update session member, members is missing : {model}.");

            var session = RequestSession(model);
            session.Members.Replace(members);
            await runtimeBackupService.BackupAsync();
        }

        public async Task RemoveSession(RequestSession model)
        {
            runtimeRepository.Remove(model.SessionId);
            await runtimeBackupService.BackupAsync(true);
        }
        
        public async Task RemoveStage(RequestStage model)
        {
            var session = RequestSession(model);
            var stage = session.Stages.FirstOrDefault(x => x.Id == model.StageId);
            session.Stages.Remove(stage);
            session.Stages.SortIfOrderable();
            await runtimeBackupService.BackupAsync(true);
        }
        
        public async Task RemoveGroup(RequestGroup model)
        {
            var stage = RequestStage(model);
            var group = stage.Groups.FirstOrDefault(x => x.Id == model.GroupId);
            stage.Groups.Remove(group);
            stage.Groups.SortIfOrderable();
            await runtimeBackupService.BackupAsync(true);
        }
        
        public async Task RemoveGame(RequestGame model)
        {
            var group = RequestGroup(model);
            var game = RequestGame(model);
            group.Games.Remove(game);
            group.Games.SortIfOrderable();
            await runtimeBackupService.BackupAsync(true);
        }
        
        public async Task RemoveMember(ReceiveSessionData model)
        {
            if (!model.Data.TryGetValue(out RuntimeMember[] members))
                throw new ClientException($"Can't remove session member, members is missing : {model}.");
            
            var session = RequestSession(model);
            members.Foreach(member => session.Members.Remove(session.Members.FirstOrDefault(x=> x.Id == member.Id)));
            await runtimeBackupService.BackupAsync(true);
        }

        private RuntimeSession RequestSession(RequestSession model)
        {
            var session = runtimeRepository.Get(model.SessionId);
            if (session == null)
                throw new NotFoundException($"Session with id : {model.SessionId} doesn't exist.\n" +
                                            $"Get Request : {model}");
            return session;
        }

        private RuntimeStage RequestStage(RequestStage model)
        {
            var session = RequestSession(model);
            var stage = session.Stages.FirstOrDefault(x => x.Id == model.StageId);

            if (stage == null)
                throw new NotFoundException($"Stage with id : {model.StageId} doesn't exist.\n" +
                                            $"Get Request : {model}");
            return stage;
        }

        private RuntimeGroup RequestGroup(RequestGroup model)
        {
            var stage = RequestStage(model);
            var group = stage.Groups.FirstOrDefault(x => x.Id == model.GroupId);
            if (group == null)
                throw new NotFoundException($"Group with id : {model.GroupId} doesn't exist.\n" +
                                            $"Get Request : {model}");
            return group;
        }

        private RuntimeGame RequestGame(RequestGame model)
        {
            var group = RequestGroup(model);
            var game = group.Games.FirstOrDefault(x => x.Id == model.GameId);
            if (game == null)
                throw new NotFoundException($"Game with id : {model.GameId} doesn't exist.\n" +
                                          $"Get Request : {model}");
            return game;
        }
    }

}
using System.Dynamic;
using LoLTournaments.Application.Exceptions;
using LoLTournaments.Application.Runtime;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Application.Services
{

    public interface ILobbyService
    {
        Task<dynamic> GetRooms();
        Task<dynamic> GetRoom(RequestSession model);
        Task<dynamic> GetRoomData(RequestSessionData model);
        Task SetRoomData(ReceiveSessionData model);
        Task RemoveRoom(RequestSession model);
        Task UpdateRoom(ReceiveSessionData model);
        Task RemoveRegistration(ReceiveSessionData model);
        Task UpdateRegistration(ReceiveSessionData model);
        Task RemoveAcception(ReceiveSessionData model);
        Task UpdateAcception(ReceiveSessionData model);
    }

    public class LobbyService : ILobbyService
    {
        private readonly IRuntimeRepository<RuntimeRoom> runtimeRepository;

        public LobbyService(IRuntimeRepository<RuntimeRoom> runtimeRepository)
        {
            this.runtimeRepository = runtimeRepository;
        }

        public Task<dynamic> GetRooms()
        {
            return Task.FromResult<dynamic>(runtimeRepository.Get().ToList().SortIfOrderable());
        }

        public Task<dynamic> GetRoom(RequestSession model)
        {
            return Task.FromResult<dynamic>(RequestRoom(model));
        }

        public Task<dynamic> GetRoomData(RequestSessionData model)
        {
            var room = RequestRoom(model);

            // ReSharper disable once HeapView.BoxingAllocation
            object result = model.PropertyName switch
            {
                nameof(room.Accepted) => room.Accepted,
                nameof(room.Info) => room.Info.SortIfOrderable(),
                nameof(room.Registered) => room.Registered,
                nameof(room.Name) => room.Name,
                nameof(room.Order) => room.Order,
                nameof(room.State) => room.State,
                nameof(room.Timer) => room.Timer,
                nameof(room.HasChanges) => room.HasChanges,
                nameof(room.StartDate) => room.StartDate,
                nameof(room.Id) => room.Id,
                nameof(room.Version) => room.Version,
                _ => throw new ClientException(
                    $"Get room data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]")
            };
            
            var resultObj = new ExpandoObject();
            resultObj.TryAdd(model.PropertyName, result);
            return Task.FromResult<dynamic>(resultObj);
        }

        public Task SetRoomData(ReceiveSessionData model)
        {
            var room = RequestRoom(model);

            switch (model.PropertyName)
            {
                case nameof(room.Accepted):
                {
                    room.Accepted.Clear();
                    model.Data.GetValue<string[]>().Foreach(room.Accepted.Add); 
                    break;
                }
                case nameof(room.Info):
                {
                    room.Info.Clear();
                    model.Data.GetValue<ParamInfo[]>().Foreach(room.Info.Add); 
                    break;
                }
                case nameof(room.Registered):
                {
                    room.Registered.Clear();
                    model.Data.GetValue<string[]>().Foreach(room.Registered.Add); 
                    break;
                }
                case nameof(room.Name): room.Name = model.Data.GetValue(room.Name); break;
                case nameof(room.Order): room.Order = model.Data.GetValue(room.Order); break;
                case nameof(room.State): room.State = model.Data.GetValue(room.State); break;
                case nameof(room.Timer): room.Timer = model.Data.GetValue(room.Timer); break;
                case nameof(room.HasChanges): room.HasChanges = model.Data.GetValue(room.HasChanges); break;
                case nameof(room.StartDate): room.StartDate = model.Data.GetValue(room.StartDate); break;
                case nameof(room.Id): room.Id = model.Data.GetValue(room.Id); break;
                case nameof(room.Version): room.Version = model.Data.GetValue(room.Version); break;
                default: throw new ClientException($"Set room data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]");
            } 
            
            return Task.CompletedTask;
        }

        public Task RemoveRoom(RequestSession model)
        {
            runtimeRepository.Remove(model.SessionId);
            return Task.CompletedTask;
        }

        public Task UpdateRoom(ReceiveSessionData model)
        {
            if (!model.Data.TryGetValue(out RuntimeRoom[] rooms))
                throw new ClientException($"Can't update rooms, rooms is missing : {model}.");

            runtimeRepository.Replace(rooms);
            return Task.CompletedTask;
        }
        
        public Task RemoveRegistration(ReceiveSessionData model)
        {
            var room = RequestRoom(model);
            
            if (!model.Data.TryGetValue(out string[] memberIds))
                throw new ClientException($"Can't UnRegistration, missing members.\n" +
                                          $"Request : {model}");

            memberIds.Foreach(id => room.Registered.Remove(id));
            return Task.CompletedTask;
        }
        
        public Task UpdateRegistration(ReceiveSessionData model)
        {
            var room = RequestRoom(model);
            
            if (!model.Data.TryGetValue(out string[] memberIds))
                throw new ClientException($"Can't update registration, missing members.\n" +
                                          $"Request : {model}");
            
            room.Registered.Replace(memberIds);
            return Task.CompletedTask;
        }
        
        public Task RemoveAcception(ReceiveSessionData model)
        {
            var room = RequestRoom(model);
            
            if (!model.Data.TryGetValue(out string[] memberIds))
                throw new ClientException($"Can't remove acception, missing members.\n" +
                                          $"Request : {model}");
            
            memberIds.Foreach(id => room.Accepted.Remove(id));
            return Task.CompletedTask;
        }
        
        public Task UpdateAcception(ReceiveSessionData model)
        {
            var room = RequestRoom(model);
            
            if (!model.Data.TryGetValue(out string[] memberIds))
                throw new ClientException($"Can't update acception, missing members.\n" +
                                          $"Request : {model}");

            room.Accepted.Replace(memberIds);
            return Task.CompletedTask;
        }

        private RuntimeRoom RequestRoom(RequestSession model)
        {
            var room = runtimeRepository.Get(model.SessionId);
            if (room == null)
                throw new NotFoundException($"Room with id : {model.SessionId} doesn't exist.\n" + 
                                            $"Get Request : {model}");
            return room;
        }
    }

}
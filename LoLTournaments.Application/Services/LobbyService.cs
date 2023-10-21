using AutoMapper;
using LoLTournaments.Application.Abstractions;
using LoLTournaments.Application.Exceptions;
using LoLTournaments.Application.Models;
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Application.Services
{

    public interface ILobbyService
    {
        Task<Room[]> GetRooms();
        Task<Room> GetRoom(RequestSession model);
        Task<object> GetRoomData(RequestSessionData model);
        Task SetRoomData(ReceiveSessionData model);
        Task RemoveRoom(RequestSession model);
        Task UpdateRooms(ReceiveSessionData model);
        Task RemoveRegistration(ReceiveSessionData model);
        Task UpdateRegistration(ReceiveSessionData model);
        Task RemoveAcception(ReceiveSessionData model);
        Task UpdateAcception(ReceiveSessionData model);
    }

    public class LobbyService : ILobbyService
    {
        private readonly IMapper mapper;
        private readonly IRuntimeRepository<RuntimeRoom> runtimeRepository;

        public LobbyService(
            IMapper mapper,
            IRuntimeRepository<RuntimeRoom> runtimeRepository)
        {
            this.mapper = mapper;
            this.runtimeRepository = runtimeRepository;
            runtimeRepository.Add(new RuntimeRoom{Id = "1"});
            runtimeRepository.Add(new RuntimeRoom{Id = "2"});
        }

        public Task<Room[]> GetRooms()
        {
            return Task.FromResult(mapper.Map<Room[]>(runtimeRepository.Get()));
        }

        public Task<Room> GetRoom(RequestSession model)
        {
            return Task.FromResult(mapper.Map<Room>(runtimeRepository.Get(model.SessionId)));
        }

        public Task<object> GetRoomData(RequestSessionData model)
        {
            var room = runtimeRepository.Get(model.SessionId);
            if (room == null)
                throw new ClientException($"Room with id : {model.SessionId} doesn't exist.\n" +
                                          $"Get Request : {model}");

            var dataResult = room.GetData(model.PropertyName);
            if (!dataResult)
                throw new ClientException(dataResult.Error);

            return Task.FromResult(dataResult.Value);
        }

        public Task SetRoomData(ReceiveSessionData model)
        {
            var room = runtimeRepository.Get(model.SessionId);
            if (room == null)
                throw new ClientException($"Room with id : {model.SessionId} doesn't exist. Get Request : {model}");

            switch (model.PropertyName)
            {
                case nameof(room.Accepted):
                {
                    room.Accepted.Clear();
                    model.GetValue<IEnumerable<string>>().Foreach(room.Accepted.Add); 
                    break;
                }
                case nameof(room.Info):
                {
                    room.Info.Clear();
                    model.GetValue<IEnumerable<ParamInfo>>().Foreach(room.Info.Add); 
                    break;
                }
                case nameof(room.Registred):
                {
                    room.Registred.Clear();
                    model.GetValue<IEnumerable<string>>().Foreach(room.Registred.Add); 
                    break;
                }
                case nameof(room.Name): room.Name = model.GetValue(room.Name); break;
                case nameof(room.Order): room.Order = model.GetValue(room.Order); break;
                case nameof(room.State): room.State = model.GetValue(room.State); break;
                case nameof(room.Timer): room.Timer = model.GetValue(room.Timer); break;
                case nameof(room.HasChanges): room.HasChanges = model.GetValue(room.HasChanges); break;
                case nameof(room.StartDate): room.StartDate = model.GetValue(room.StartDate); break;
                case nameof(room.Id): room.Id = model.GetValue(room.Id); break;
                case nameof(room.Version): room.Version = model.GetValue(room.Version); break;
                default: throw new ClientException($"Set room data operation failed, Unknown [{nameof(model.PropertyName)} : {model.PropertyName}]");
            } 
            
            return Task.CompletedTask;
        }

        public Task RemoveRoom(RequestSession model)
        {
            runtimeRepository.Remove(model.SessionId);
            return Task.CompletedTask;
        }

        public Task UpdateRooms(ReceiveSessionData model)
        {
            if (model.GetValue<IEnumerable<Room>>() is not {} rooms)
                throw new ClientException($"Can't update rooms, rooms is missing : {model}.");

            runtimeRepository.Replace(mapper.Map<IEnumerable<RuntimeRoom>>(rooms));
            return Task.CompletedTask;
        }
        
        public Task RemoveRegistration(ReceiveSessionData model)
        {
            var room = runtimeRepository.Get(model.SessionId);
            if (room == null)
                throw new ClientException($"Room with id : {model.SessionId} doesn't exist.\n" +
                                          $"Request : {model}");
            
            if (model.GetValue<IEnumerable<string>>() is not {} memberIds)
                throw new ClientException($"Can't UnRegistration, missing members.\n" +
                                          $"Request : {model}");

            memberIds.Foreach(id => room.Registred.Remove(id));
            return Task.CompletedTask;
        }
        
        public Task UpdateRegistration(ReceiveSessionData model)
        {
            var room = runtimeRepository.Get(model.SessionId);
            if (room == null)
                throw new ClientException($"Room with id : {model.SessionId} doesn't exist.\n" +
                                          $"Request : {model}");
            
            if (model.GetValue<IEnumerable<string>>() is not {} memberIds)
                throw new ClientException($"Can't update registration, missing members.\n" +
                                          $"Request : {model}");
            
            foreach (var memberId in memberIds)
            {
                if (!room.Registred.Contains(memberId))
                    room.Registred.Add(memberId);
            }
            
            return Task.CompletedTask;
        }
        
        public Task RemoveAcception(ReceiveSessionData model)
        {
            var room = runtimeRepository.Get(model.SessionId);
            if (room == null)
                throw new ClientException($"Room with id : {model.SessionId} doesn't exist.\n" +
                                          $"Request : {model}");
            
            if (model.GetValue<IEnumerable<string>>() is not {} memberIds)
                throw new ClientException($"Can't remove acception, missing members.\n" +
                                          $"Request : {model}");
            
            memberIds.Foreach(id => room.Accepted.Remove(id));
            return Task.CompletedTask;
        }
        
        public Task UpdateAcception(ReceiveSessionData model)
        {
            var room = runtimeRepository.Get(model.SessionId);
            if (room == null)
                throw new ClientException($"Room with id : {model.SessionId} doesn't exist.\n" +
                                          $"Request : {model}");
            
            if (model.GetValue<IEnumerable<string>>() is not {} memberIds)
                throw new ClientException($"Can't update acception, missing members.\n" +
                                          $"Request : {model}");
            
            foreach (var memberId in memberIds)
            {
                if (!room.Accepted.Contains(memberId))
                    room.Accepted.Add(memberId);
            }
            
            return Task.CompletedTask;
        }
    }

}
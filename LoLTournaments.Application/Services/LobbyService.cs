using LoLTournaments.Application.Abstractions;
using LoLTournaments.Application.Exceptions;
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
        private readonly IRuntimeRepository<Room> runtimeRepository;

        public LobbyService(IRuntimeRepository<Room> runtimeRepository)
        {
            this.runtimeRepository = runtimeRepository;
            runtimeRepository.Add(new Room{Id = "1"});
            runtimeRepository.Add(new Room{Id = "2"});
        }

        public Task<Room[]> GetRooms()
        {
            return Task.FromResult(runtimeRepository.Get());
        }

        public Task<Room> GetRoom(RequestSession model)
        {
            return Task.FromResult(runtimeRepository.Get(model.SessionId));
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
                case nameof(room.Accepted): room.Accepted = model.GetValue(room.Accepted); break;
                case nameof(room.Info): room.Info = model.GetValue(room.Info); break;
                case nameof(room.Name): room.Name = model.GetValue(room.Name); break;
                case nameof(room.Order): room.Order = model.GetValue(room.Order); break;
                case nameof(room.Registred): room.Registred = model.GetValue(room.Registred); break;
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

            runtimeRepository.Replace(rooms);
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

            room.Registred.RemoveAll(memberId => memberIds.Contains(memberId));
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
        
            room.Registred.AddRange(memberIds.Except(room.Registred));
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

            room.Accepted.RemoveAll(memberId => memberIds.Contains(memberId));
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
        
            room.Accepted.AddRange(memberIds.Except(room.Registred));
            return Task.CompletedTask;
        }
    }

}
using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;
using Timer = LoLTournaments.Shared.Models.Timer;

namespace LoLTournaments.Application.Sessions
{

    public class LobbyContext : Context<Room>
    {
        // public Result<object> GetRegistered(string sessionId)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return (Result)roomResult;
        //
        //     return new{roomResult.Value.Registred};
        // }
        //
        // public Result<object> GetAccepted(string sessionId)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return (Result)roomResult;
        //
        //     return new{roomResult.Value.Accepted};
        // }
        //
        // public Result<object> GetInfos(string sessionId)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return (Result)roomResult;
        //
        //     return new{roomResult.Value.Info};
        // }
        //
        // public Result<object> GetStartDate(string sessionId)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return (Result)roomResult;
        //
        //     return new{roomResult.Value.StartDate};
        // }
        //
        // public Result<object> GetState(string sessionId)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return (Result)roomResult;
        //
        //     return new{roomResult.Value.State};
        // }
        //
        // public Result<object> GetTimer(string sessionId)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return (Result)roomResult;
        //
        //     return new{roomResult.Value.Timer};
        // }
        //
        // public Result<object> GetName(string sessionId)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return (Result)roomResult;
        //
        //     return new{roomResult.Value.Name};
        // }
        //
        // public Result UpdateTimer(string sessionId, Timer dto)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return roomResult;
        //
        //     roomResult.Value.Timer = dto;
        //     return Result.Success();
        // }
        //
        // public Result UpdateName(string sessionId, string value)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return roomResult;
        //
        //     roomResult.Value.Name = value;
        //     return Result.Success();
        // }
        //
        // public Result RemoveRoom(string sessionId)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return roomResult;
        //
        //     ContextItems.Remove(roomResult.Value);
        //     return Result.Success();
        // }
        //
        // public Result RemoveTimer(string sessionId)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return roomResult;
        //
        //     roomResult.Value.Timer = null;
        //     return Result.Success();
        // }
        //
        // public Result UpdateRoom(Room room)
        // {
        //     if (room == null)
        //         return Result.Failure("Can't update, room is missing.");
        //
        //     ContextItems.Replace(room);
        //     return Result.Success();
        // }
        //
        // public Result UpdateState(string sessionId, LobbyState state)
        // {
        //     var roomResult = GetRoom(sessionId);
        //     if (!roomResult)
        //         return roomResult;
        //     
        //     roomResult.Value.State = state;
        //     return Result.Success();
        // }
        //
        // public Result RemoveRegistration(string userId)
        // {
        //     if (!IsValidCurrent())
        //         return Result.Failure("Can't remove user registration : room not found");
        //
        //     Current.Registred.RemoveAll(x => x == userId);
        //     return Result.Success();
        // }
        //
        // public Result RemoveRegistrations(IEnumerable<string> users)
        // {
        //     if (!IsValidCurrent())
        //         return Result.Failure("Can't remove users registration : room not found");
        //
        //     if (users == null)
        //         return Result.Success();
        //
        //     Current.Registred = Current.Registred.Except(users).ToListFix();
        //     return Result.Success();
        // }
        //
        // public Result AddRegistration(string userId)
        // {
        //     if (!IsValidCurrent())
        //         return Result.Failure("Can't add user registration : room not found");
        //
        //     if (Current.Registred.Contains(userId))
        //         return Result.Success();
        //
        //     Current.Registred.Add(userId);
        //     return Result.Success();
        // }
        //
        // public Result UpdateRegistrations(IEnumerable<string> users)
        // {
        //     if (!IsValidCurrent())
        //         return Result.Failure("Can't update user registration : room not found");
        //
        //     if (users == null)
        //         return Result.Failure("Can't update users registration : users not found");
        //
        //     Current.Registred = Current.Registred.Union(users).Distinct().ToList();
        //     return Result.Success();
        // }
        //
        // public Result RemoveAcception(string userId)
        // {
        //     if (!IsValidCurrent())
        //         return Result.Failure("Can't remove user acception : room not found");
        //
        //     Current.Accepted.Remove(userId);
        //     return Result.Success();
        // }
        //
        // public Result AddAcception(string userId)
        // {
        //     if (!IsValidCurrent())
        //         return Result.Failure("Can't add user acception : room not found");
        //
        //     if (Current.Accepted.Contains(userId))
        //         return Result.Success();
        //
        //     Current.Accepted.Add(userId);
        //     return Result.Success();
        // }
        //
        // public Result UpdateAcceptions(IEnumerable<string> users)
        // {
        //     if (!IsValidCurrent())
        //         return Result.Failure("Can't update users acception : room not found");
        //
        //
        //     if (users == null)
        //         return Result.Failure("Can't update users acception : users not found");
        //
        //     var toAcception = users.Except(Current.Accepted).ToListFix();
        //     Current.Accepted = Current.Accepted.Union(toAcception).Distinct().ToList();
        //     return Result.Success();
        // }
        //
        // public Result RemoveAcceptions(IEnumerable<string> users)
        // {
        //     if (!IsValidCurrent())
        //         return Result.Failure("Can't remove users acception : room not found");
        //
        //
        //     if (users == null)
        //         return Result.Failure("Can't remove users acception : users not found");
        //
        //     Current.Accepted = Current.Accepted.Except(users).ToListFix();
        //     return Result.Success();
        // }
        //
        // public Result UpdateRooms(IEnumerable<Room> rooms)
        // {
        //     if (rooms == null)
        //         return Result.Failure("Can't update rooms, rooms is missing");
        //
        //     ContextItems.Replace(rooms);
        //     RequestToRefreshCurrent();
        //
        //     return Result.Success();
        // }
        //
        // public Result<Room> GetRoom(string sessionId)
        // {
        //     var room = ContextItems.FirstOrDefault(x => x.Id == sessionId);
        //     if (room == null)
        //         return Result.Failure($"Room with id : {sessionId} doesn't exist.");
        //
        //     return room;
        // }
    }

}
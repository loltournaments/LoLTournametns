using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;
using Timer = LoLTournaments.Shared.Models.Timer;

namespace LoLTournaments.Application.Sessions
{

    public class LobbyContext : Context<Room>
    {
        public Result<List<string>> GetRegistered()
        {
            if (!IsValidCurrent())
                    return Result.Failure("Can't get room registered : room not found");

            return Current.Registred;
        }

        public Result<List<string>> GetAccepted()
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't get room accepted : room not found");

            return Current.Accepted;
        }

        public Result<List<ParamInfo>> GetInfos()
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't get room infos : room not found");

            return Current.Info;
        }

        public Result<DateTime> GetStartDate()
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't get room start date : room not found");

            return Current.StartDate;
        }

        public Result<LobbyState> GetState()
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't get room state : room not found");

            return Current.State;
        }

        public Result<Timer> GetTimer()
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't get room timer : room not found");

            return Current.Timer;
        }

        public Result<string> GetName()
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't get room name : room not found");

            return Current.Name;
        }
        
        public Result UpdateTimer(Timer dto)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't update room timer : room not found");

            Current.Timer = dto;
            return Result.Success();
        }

        public Result UpdateName(string value)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't update room name : room not found");

            Current.Name = value;
            return Result.Success();
        }
        
        public Result RemoveRoom()
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't remove room : room not found");

            ContextItems.Remove(Current);
            RequestToRefreshCurrent();
            return Result.Success();
        }

        public Result RemoveTimer()
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't remove timer : room not found");

            Current.Timer = null;
            return Result.Success();
        }
        
        public Result UpdateRoom(Room room)
        {
            if (room == null)
                return Result.Failure("Room can't update");

            ContextItems.Replace(room);
            RequestToRefreshCurrent();

            return Result.Success();
        }

        public Result UpdateState(LobbyState state)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't update room state : room not found");

            Current.State = state;
            return Result.Success();
        }

        public Result RemoveRegistration(string userId)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't remove user registration : room not found");

            Current.Registred.RemoveAll(x => x == userId);
            return Result.Success();
        }

        public Result RemoveRegistrations(IEnumerable<string> users)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't remove users registration : room not found");

            if (users == null)
                return Result.Success();

            Current.Registred = Current.Registred.Except(users).ToListFix();
            return Result.Success();
        }

        public Result AddRegistration(string userId)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't add user registration : room not found");

            if (Current.Registred.Contains(userId))
                return Result.Success();

            Current.Registred.Add(userId);
            return Result.Success();
        }

        public Result UpdateRegistrations(IEnumerable<string> users)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't update user registration : room not found");

            if (users == null)
                return Result.Failure("Can't update users registration : users not found");

            Current.Registred = Current.Registred.Union(users).Distinct().ToList();
            return Result.Success();
        }

        public Result RemoveAcception(string userId)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't remove user acception : room not found");

            Current.Accepted.Remove(userId);
            return Result.Success();
        }

        public Result AddAcception(string userId)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't add user acception : room not found");

            if (Current.Accepted.Contains(userId))
                return Result.Success();

            Current.Accepted.Add(userId);
            return Result.Success();
        }

        public Result UpdateAcceptions(IEnumerable<string> users)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't update users acception : room not found");


            if (users == null)
                return Result.Failure("Can't update users acception : users not found");

            var toAcception = users.Except(Current.Accepted).ToListFix();
            Current.Accepted = Current.Accepted.Union(toAcception).Distinct().ToList();
            return Result.Success();
        }

        public Result RemoveAcceptions(IEnumerable<string> users)
        {
            if (!IsValidCurrent())
                return Result.Failure("Can't remove users acception : room not found");


            if (users == null)
                return Result.Failure("Can't remove users acception : users not found");

            Current.Accepted = Current.Accepted.Except(users).ToListFix();
            return Result.Success();
        }
        
        public Result UpdateRooms(IEnumerable<Room> rooms)
        {
            if (rooms == null)
                return Result.Failure("Can't update rooms, rooms is missing");

            ContextItems.Replace(rooms);
            RequestToRefreshCurrent();

            return Result.Success();
        }
    }

}
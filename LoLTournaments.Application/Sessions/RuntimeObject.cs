using LoLTournaments.Shared.Models;

namespace LoLTournaments.Application.Sessions
{

    public class RuntimeObject
    {
        public Session Session { get; }
        public Room Room { get; }

        public RuntimeObject()
        {
            Session = new Session();
            Room = new Room();
        }
    }

}
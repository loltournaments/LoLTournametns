using LoLTournaments.Shared.Models;
using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Application.Services
{

    public interface ILobbyService
    {
        Task<object> GetData(string request);
    }
    public class LobbyService : ILobbyService
    {
        private readonly Room room;
        public LobbyService()
        {
            room = new Room();
        }

        public Task<object> GetData(string request)
        {
            var result = room.GetData(request);
            if (!result)
                throw new Exception(result.Error);
            
            return Task.FromResult(result.Value);
        }
    }

}
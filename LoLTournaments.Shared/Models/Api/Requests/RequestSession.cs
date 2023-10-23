using LoLTournaments.Shared.Utilities;

namespace LoLTournaments.Shared.Models
{

    public class RequestSession
    {
        public string SessionId { get; set; }
        

        public override string ToString()
        {
            return this.ReflectionFormat();
        }
    }

}
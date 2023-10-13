namespace LoLTournaments.Application.Services
{

    public interface IIdentityService
    {
        Task Login();
        Task Register();
        Task Authenticate();
        Task ResetPassword();
    }
    public class IdentityService : IIdentityService
    {
        public Task Login()
        {
            throw new NotImplementedException();
        }

        public Task Register()
        {
            throw new NotImplementedException();
        }

        public Task Authenticate()
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword()
        {
            throw new NotImplementedException();
        }
    }

}
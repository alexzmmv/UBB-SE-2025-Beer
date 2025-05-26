using DataAccess.Model.Authentication;
using DataAccess.OAuthProviders;
using WinUiApp.Data.Data;

namespace DataAccess.Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthWithUserPass(string username, string password);

        Task<AuthenticationResponse> AuthWithOAuth(OAuthService selectedService, object authProvider);

        Task<User?> GetUser(Guid sessionId);

        void Logout();
    }
}
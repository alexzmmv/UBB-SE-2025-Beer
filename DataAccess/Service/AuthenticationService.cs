using System.Security.Cryptography;
using DataAccess.AuthProviders;
using DataAccess.AuthProviders.Facebook;
using DataAccess.AuthProviders.Github;
using DataAccess.AuthProviders.LinkedIn;
using DataAccess.Model.Authentication;
using DataAccess.OAuthProviders;
using static DataAccess.AuthProviders.BasicAuthenticationProvider;
using DataAccess.Service.Interfaces;
using WinUiApp.Data.Data;
using DataAccess.Constants;
using DataAccess.IRepository;

namespace DataAccess.Service
{
    public enum OAuthService
    {
        None,
        Google,
        Facebook,
        Twitter,
        GitHub,
        LinkedIn
    }

    public class AuthenticationService : IAuthenticationService
    {
        private ISessionRepository sessionRepository;
        private IUserRepository userRepository;
        private ILinkedInLocalOAuthServer linkedinLocalServer;
        private IGitHubLocalOAuthServer githubLocalServer;
        private IFacebookLocalOAuthServer facebookLocalServer;
        private IBasicAuthenticationProvider basicAuthenticationProvider;
        private static Guid currentSessionId = Guid.Empty;
        private static Guid currentUserId = Guid.Empty;

        public AuthenticationService(ISessionRepository sessionRepository, IUserRepository userRepository, ILinkedInLocalOAuthServer linkedinLocalServer,
            IGitHubLocalOAuthServer githubLocalServer, IFacebookLocalOAuthServer facebookLocalServer, IBasicAuthenticationProvider basicAuthenticationProvider)
        {
            this.sessionRepository = sessionRepository;
            this.userRepository = userRepository;
            this.linkedinLocalServer = linkedinLocalServer;
            this.githubLocalServer = githubLocalServer;
            this.facebookLocalServer = facebookLocalServer;
            this.basicAuthenticationProvider = basicAuthenticationProvider;

            _ = this.githubLocalServer.StartAsync();
            _ = this.facebookLocalServer.StartAsync();
            _ = this.linkedinLocalServer.StartAsync();
        }

        public async Task<AuthenticationResponse> AuthWithOAuth(OAuthService selectedService, object authProvider)
        {
            switch (selectedService)
            {
                /*case OAuthService.Google:
                    break;*/
                case OAuthService.Facebook:
                    return await AuthenticateWithFacebookAsync((IFacebookOAuthHelper)authProvider);
                /*case OAuthService.Twitter:
                    break;*/
                case OAuthService.GitHub:
                    return await AuthenticateWithGitHubAsync((IGitHubOAuthHelper)authProvider);
                case OAuthService.LinkedIn:
                    return await AuthenticateWithLinkedInAsync((ILinkedInOAuthHelper)authProvider);
                default:
                    return new AuthenticationResponse { AuthenticationSuccessful = false, NewAccount = false, OAuthToken = string.Empty, SessionId = Guid.Empty };
            }
        }

        public virtual void Logout()
        {
            this.sessionRepository.EndSession(currentSessionId);
            currentSessionId = Guid.Empty;
            currentUserId = Guid.Empty;
        }

        public virtual async Task<User?> GetUser(Guid sessionId)
        {
            Session? session = await this.sessionRepository.GetSession(sessionId);

            if (session == null)
            {
                return null;
            }

            return await this.userRepository.GetUserById(session.UserId);
        }

        public async Task<AuthenticationResponse> AuthWithUserPass(string username, string password)
        {
            try
            {
                if (await this.basicAuthenticationProvider.AuthenticateAsync(username, password))
                {
                    User? user = await this.userRepository.GetUserByUsername(username);

                    if (user == null)
                    {
                        throw new UserNotFoundException("Go to catch");
                    }
                    Session session = await this.sessionRepository.CreateSession(user.UserId);
                    return new AuthenticationResponse
                    {
                        AuthenticationSuccessful = true,
                        NewAccount = false,
                        OAuthToken = string.Empty,
                        SessionId = session.SessionId,
                    };
                }
                else
                {
                    return new AuthenticationResponse
                    {
                        AuthenticationSuccessful = false,
                        NewAccount = false,
                        OAuthToken = string.Empty,
                        SessionId = Guid.Empty,
                    };
                }
            }
            catch (UserNotFoundException)
            {
                User user = new ()
                {
                    Username = username,
                    PasswordHash = Convert.ToBase64String(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password)) ?? throw new Exception("Hashing failed")),
                    UserId = Guid.NewGuid(),
                    TwoFASecret = string.Empty,
                    AssignedRole = RoleType.User,
                    NumberOfDeletedReviews = 0,
                    EmailAddress = "ionutcora66@gmail.com"
                };

                await this.userRepository.CreateUser(user);
                Session session = await this.sessionRepository.CreateSession(user.UserId);
                return new AuthenticationResponse
                {
                    AuthenticationSuccessful = true,
                    NewAccount = true,
                    OAuthToken = string.Empty,
                    SessionId = session.SessionId,
                };
            }
        }

        private static async Task<AuthenticationResponse> AuthenticateWithGitHubAsync(IGitHubOAuthHelper gitHubHelper)
        {
            return await gitHubHelper.AuthenticateAsync();
        }

        private static async Task<AuthenticationResponse> AuthenticateWithFacebookAsync(IFacebookOAuthHelper faceBookHelper)
        {
            return await faceBookHelper.AuthenticateAsync();
        }

        private static async Task<AuthenticationResponse> AuthenticateWithLinkedInAsync(ILinkedInOAuthHelper linkedInHelper)
        {
            return await linkedInHelper.AuthenticateAsync();
        }

        public static Guid GetCurrentSessionId()
        {
            return currentSessionId;
        }

        public static Guid GetCurrentUserId()
        {
            return currentUserId;
        }

        public static void SetCurrentSessionId(Guid sessionId)
        {
            currentSessionId = sessionId;
        }

        public static void SetCurrentUserId(Guid userId)
        {
            currentUserId = userId;
        }
    }
}

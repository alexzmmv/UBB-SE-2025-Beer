using System.Security.Cryptography;
using System.Text;
using DataAccess.Model.Authentication;
using DataAccess.Service.Interfaces;
using WinUiApp.Data.Data;

namespace DataAccess.AuthProviders
{
    public class BasicAuthenticationProvider : IBasicAuthenticationProvider
    {
        public class UserNotFoundException : Exception
        {
            public UserNotFoundException(string message) : base(message)
            {
            }
        }

        private IUserService userService;
        public BasicAuthenticationProvider(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            User? user = await userService.GetUserByUsername(username);
            if (user == null)
            {
                throw new UserNotFoundException("User not found!");
            }

            string hashedPassword = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
            bool isAuthenticated = user.PasswordHash == hashedPassword;
            return isAuthenticated;
        }

        // Keep the sync method for backward compatibility, but make it use the async version
        public bool Authenticate(string username, string password)
        {
            return AuthenticateAsync(username, password).GetAwaiter().GetResult();
        }
    }
}

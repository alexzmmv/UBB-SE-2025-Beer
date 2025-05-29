namespace DataAccess.Service
{
    using System;
    using DataAccess.Model.Authentication;
    using DataAccess.Service.Components;
    using DataAccess.Service.Interfaces;
    using IRepository;
    using OtpNet;
    using WinUiApp.Data.Data;

    public class TwoFactorAuthenticationService : ITwoFactorAuthenticationService
    {
        private IUserRepository userRepository;
        private IKeyGeneration keyGeneration = new OtpKeyGeneration();
        private User? currentUser;

        public Guid UserId { get; set; }
        public bool IsFirstTimeSetup { get; set; }

        public TwoFactorAuthenticationService(IUserRepository userRepo)
        {
            userRepository = userRepo;
        }

        public async Task<(User? currentUser, string uniformResourceIdentifier, byte[] twoFactorSecret)> Get2FAValues()
        {
            try
            {
                currentUser = await userRepository.GetUserById(UserId) ?? throw new ArgumentException("User not found.");

                int keyLength = 42;
                byte[] twoFactorSecret;
                string uniformResourceIdentifier = string.Empty;

                if (IsFirstTimeSetup)
                {
                    twoFactorSecret = keyGeneration?.GenerateRandomKey(keyLength) ?? throw new InvalidOperationException("Failed to generate 2FA secret.");
                    currentUser.TwoFASecret = Convert.ToBase64String(twoFactorSecret);
                    await userRepository.UpdateUser(currentUser);
                    uniformResourceIdentifier = new OtpUri(OtpType.Totp, twoFactorSecret, currentUser.Username, "DrinkDB").ToString();
                }
                else
                {
                    twoFactorSecret = Convert.FromBase64String(currentUser.TwoFASecret ?? string.Empty);
                }

                return (currentUser, uniformResourceIdentifier, twoFactorSecret);
            }
            catch
            {
                return (null, string.Empty, Array.Empty<byte>());
            }
        }
    }
}
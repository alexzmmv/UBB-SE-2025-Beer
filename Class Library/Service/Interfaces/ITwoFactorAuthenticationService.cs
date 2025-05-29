namespace DataAccess.Service.Interfaces
{
    using DataAccess.Model.Authentication;
    using WinUiApp.Data.Data;

    public interface ITwoFactorAuthenticationService
    {
        Guid UserId { get; set; }
        bool IsFirstTimeSetup { get; set; }

        Task<(User? currentUser, string uniformResourceIdentifier, byte[] twoFactorSecret)> Get2FAValues();
    }
}

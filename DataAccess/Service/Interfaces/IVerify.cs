﻿namespace DataAccess.Service.Interfaces
{
    public interface IVerify
    {
        bool Verify2FAForSecret(byte[] twoFactorSecret, string token);
    }
}

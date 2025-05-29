using DataAccess.Service.Interfaces;

namespace DataAccess.Service.Components
{
    public class OtpKeyGeneration : IKeyGeneration
    {
        public byte[] GenerateRandomKey(int keyLength)
        {
            return OtpNet.KeyGeneration.GenerateRandomKey(keyLength);
        }
    }
}

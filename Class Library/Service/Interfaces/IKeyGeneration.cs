namespace DataAccess.Service.Interfaces
{
    public interface IKeyGeneration
    {
        byte[] GenerateRandomKey(int keyLength);
    }
}

using DataAccess.Model.Authentication;
using DataAccess.Service.Interfaces;
using IRepository;

namespace DataAccess.Service
{
    public class SessionService : ISessionService
    {
        private ISessionRepository sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        }

        public async Task<Session?> CreateSessionAsync(Guid userId)
        {
            try
            {
                return await sessionRepository.CreateSession(userId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> EndSessionAsync(Guid sessionId)
        {
            try
            {
                return await sessionRepository.EndSession(sessionId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<Session?> GetSessionAsync(Guid sessionId)
        {
            try
            {
                return await sessionRepository.GetSession(sessionId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Session?> GetSessionByUserIdAsync(Guid userId)
        {
            try
            {
                return await sessionRepository.GetSessionByUserId(userId);
            }
            catch
            {
                return null;
            }
        }
    }
}
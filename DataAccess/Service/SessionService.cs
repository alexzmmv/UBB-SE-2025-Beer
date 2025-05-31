using DataAccess.IRepository;
using DataAccess.Model.Authentication;
using DataAccess.Service.Interfaces;

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
                return await this.sessionRepository.CreateSession(userId);
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
                return await this.sessionRepository.EndSession(sessionId);
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
                return await this.sessionRepository.GetSession(sessionId);
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
                return await this.sessionRepository.GetSessionByUserId(userId);
            }
            catch
            {
                return null;
            }
        }
    }
}
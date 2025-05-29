using DataAccess.Model.Authentication;
using IRepository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data;
using WinUiApp.Data.Interfaces;

namespace DataAccess.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private IAppDbContext dataContext;

        public SessionRepository(IAppDbContext context)
        {
            dataContext = context;
        }
        public async Task<Session> CreateSession(Guid userId)
        {
            Session session = new Session
            {
                SessionId = Guid.NewGuid(),
                UserId = userId
            };

            await dataContext.Sessions.AddAsync(session);
            await dataContext.SaveChangesAsync();

            return session;
        }

        public async Task<bool> EndSession(Guid sessionId)
        {
            Session? session = await dataContext.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session == null)
            {
                // I consider this true if we didn't have the session itself
                return true;
            }

            dataContext.Sessions.Remove(session);
            return await dataContext.SaveChangesAsync() > 0;
        }

        public async Task<Session?> GetSession(Guid sessionId)
        {
            return await dataContext.Sessions.FirstOrDefaultAsync(item => item.SessionId == sessionId);
        }

        public async Task<Session?> GetSessionByUserId(Guid userId)
        {
            return await dataContext.Sessions.FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}
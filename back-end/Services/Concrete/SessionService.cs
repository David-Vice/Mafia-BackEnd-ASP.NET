using back_end.Data;
using back_end.Models;
using back_end.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services.Concrete
{
    public class SessionService : ISessionService
    {
        private readonly IDataContext _dataContext;

        public SessionService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Session> Get(int id)
        {
            var session = await _dataContext.Sessions.FindAsync(id);
            if (session == null)
            {
                throw new NullReferenceException();
            }

            return session;
        }

        public async Task Add(Session session)
        {
            if (session.NumberOfPlayers <= session.MaxNumberOfPlayers)
            {
                session.Admin=_dataContext.Users.Where(x=>x.Id==session.AdminId).FirstOrDefault();
                session.GameSessionsUsersRoles=_dataContext.GameSessionsUsersRoles.Where(x=>x.SessionId==session.Id).ToList();
                _dataContext.Sessions?.Add(session);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Session>> GetAll()
        {
            return await _dataContext.Sessions.ToListAsync();
        }

        public async Task Delete(int id)
        {
            var sessionToDelete = await _dataContext.Sessions.FindAsync(id);
            if (sessionToDelete == null)
            {
                throw new NullReferenceException();
            }
            _dataContext.Sessions.Remove(sessionToDelete);
            await _dataContext.SaveChangesAsync();
        }


        /// <summary>
        /// Sets end of session by changing 'endTime' to current time
        /// </summary>
        /// <param name="id">Id of the session</param>
        /// <returns></returns>
        public async Task EndSession(int id)
        {
            var sessionToEnd = await _dataContext.Sessions.FindAsync(id);
            if (sessionToEnd == null)
            {
                throw new NullReferenceException();
            }

            if (sessionToEnd.StartTime == sessionToEnd.EndTime || sessionToEnd.EndTime == null)
            {
                sessionToEnd.EndTime = DateTime.Now;
                await _dataContext.SaveChangesAsync();
            }
        }

        
        public async Task<IEnumerable<Session>> GetOpenSessions()
        {
            IEnumerable<Session> allSessions = await GetAll();
            List<Session> activeSessions = allSessions.Select(s => s).Where(s => s.StartTime.Equals(s.EndTime) || s.EndTime==null).ToList();
            return activeSessions;
        }
    }
}
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
        private readonly IGameSessionsUsersRoleService _gsurService;

        public SessionService(IDataContext dataContext, IGameSessionsUsersRoleService gsurService)
        {
            _dataContext = dataContext;
            _gsurService = gsurService;
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

        public async Task<int> Add(Session session)
        {
            if (session != null)
            {
                session.Admin=_dataContext.Users.Where(x=>x.Id==session.AdminId).FirstOrDefault();
                //session.GameSessionsUsersRoles=_dataContext.GameSessionsUsersRoles.Where(x=>x.SessionId==session.Id).ToList();
                session.StartTime = DateTime.Now;
                session.RoleAutoInitialize = 1;
                session.MaxNumberOfPlayers = 20;
                session.NumberOfPlayers = 0; // only admin
                _dataContext.Sessions?.Add(session);
                await _dataContext.SaveChangesAsync();
                await IncrementNumberOfPlayers(session.Id, session.AdminId);
                return session.Id;
            }
            else
            {
                return -1;
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
            List<Session> activeSessions = allSessions.Select(s => s).Where(s=>s.EndTime==null).ToList();
            return activeSessions;
        }

        public async Task<bool> IncrementNumberOfPlayers(int id, int userId)
        {
            Session sessionToUpdate = await _dataContext.Sessions.FindAsync(id);
            if (sessionToUpdate == null) throw new NullReferenceException();
            sessionToUpdate.NumberOfPlayers++;
            if (sessionToUpdate.NumberOfPlayers > sessionToUpdate.MaxNumberOfPlayers) return false;
            await _dataContext.SaveChangesAsync();
            GameSessionsUsersRole gsur = new GameSessionsUsersRole();
            gsur.UserId = userId;
            gsur.SessionId = id;
            gsur.RoleId = 3;
            gsur.PlayerIngameStatusId = 1;
            await _gsurService.Add(gsur);
            return true;
        }

        public async Task<bool> DecrementNumberOfPlayers(int id)
        {
            Session session = await _dataContext.Sessions.FindAsync(id);
            if (session == null) throw new NullReferenceException();
            session.NumberOfPlayers--;
            if (session.NumberOfPlayers < 0) return false;
            else if (session.NumberOfPlayers == 0)
            {
                await EndSession(id);
                return true;
            }
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
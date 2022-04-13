using back_end.Data;
using back_end.Models;
using back_end.Services.Abstract;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace back_end.Services.Concrete
{
    public class GameSessionsUsersRoleService : IGameSessionsUsersRoleService
    {
        private readonly IUserService _userService;
        private readonly IDataContext _dataContext;
        public GameSessionsUsersRoleService(IUserService userService, IDataContext dataContext)
        {
            _userService = userService;
            _dataContext = dataContext;
        }

        public async Task<GameSessionsUsersRole> Get(int id)
        {
            var gameSessions = await _dataContext.GameSessionsUsersRoles.FindAsync(id);
            if (gameSessions == null)
            {
                return null;
            }
            return gameSessions;
        }

        public async Task<IEnumerable<GameSessionsUsersRole>> GetAll()
        {
            return await _dataContext.GameSessionsUsersRoles.ToListAsync();
        }

        public async Task<List<string>> GetUsernamesBySessionId(int id)
        {
            IEnumerable<GameSessionsUsersRole> gameSessions = await GetAll();
            List<GameSessionsUsersRole> gameSessionsUsersRoles = gameSessions.Where(gs => gs.SessionId.Equals(id)).ToList();
            List<int> userids = gameSessionsUsersRoles.Select(ui => ui.UserId).ToList();
            IEnumerable<User> users = await _userService.GetAll();
            List<string> usernames = users.Where(u => userids.Contains(u.Id)).Select(u => u.UserName).ToList();

            return usernames;
        }
    }
}

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
        private readonly IRoleService _roleService;
        private readonly IDataContext _dataContext;
        public GameSessionsUsersRoleService(IUserService userService, IDataContext dataContext, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
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
        public async Task<IEnumerable<GameSessionsUsersRole>> GetBySessionId(int id)
        {
            IEnumerable<GameSessionsUsersRole> gameSessions = await GetAll();
            List<GameSessionsUsersRole> gameSessionsUsersRoles = gameSessions.Where(gs => gs.SessionId.Equals(id)).ToList();
            return gameSessionsUsersRoles;
        }

        public async Task Add(GameSessionsUsersRole gsur)
        {
            if (gsur != null)
            {
                //gsur.User = _dataContext.Users.Where(x=>x.Id == gsur.UserId).FirstOrDefault();
                //gsur.Session = _dataContext.Sessions.Where(x=>x.Id == gsur.SessionId).FirstOrDefault();
                //gsur.Role = _dataContext.Roles.Where(x => x.Id == gsur.RoleId).FirstOrDefault();
                //gsur.PlayerIngameStatus = _dataContext.PlayerIngameStatuses.Where(x => x.Id == gsur.PlayerIngameStatusId).FirstOrDefault();
                _dataContext.GameSessionsUsersRoles?.Add(gsur);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<string> DistributeRoles(int sessionId)
        {
            int mafiaCount = 0;
            IEnumerable<GameSessionsUsersRole> gameSessions = await GetAll();
            List<GameSessionsUsersRole> gameSessionsUsersRoles = gameSessions.Where(gs => gs.SessionId.Equals(sessionId)).ToList();
           // if (gameSessionsUsersRoles.Count < 5)
            //    return "Minimum 5 players required to start the game!";
            if(5<=gameSessionsUsersRoles.Count && gameSessionsUsersRoles.Count<=7)
            {
                mafiaCount = 1;
            }
            else if (8 <= gameSessionsUsersRoles.Count && gameSessionsUsersRoles.Count <= 12)
            {
                mafiaCount = 2;
            }
            else if (13 <= gameSessionsUsersRoles.Count && gameSessionsUsersRoles.Count <= 20)
            {
                mafiaCount = 3;
            }
            Random rand = new Random();
            List<GameSessionsUsersRole> shuffledGameSessionsUsersRole = gameSessionsUsersRoles.OrderBy(_ => rand.Next()).ToList();

            if (gameSessionsUsersRoles.Count >= 1) shuffledGameSessionsUsersRole[0].RoleId = 10;
            if(gameSessionsUsersRoles.Count>=2) shuffledGameSessionsUsersRole[1].RoleId = 4;
            for (int i = 2; i < 2+mafiaCount; i++)
            {
                shuffledGameSessionsUsersRole[i].RoleId = 2;
            }
            for(int i = 2+mafiaCount; i<shuffledGameSessionsUsersRole.Count; i++)
            {
                shuffledGameSessionsUsersRole[i].RoleId = 1;
            }
            for (int i = 0; i < shuffledGameSessionsUsersRole.Count; i++)
            {
                _dataContext.GameSessionsUsersRoles?.Update(shuffledGameSessionsUsersRole[i]);
            }
            await _dataContext.SaveChangesAsync();
            return "Roles are distributed!";
        }

        public async Task<string> Kill(int sessionId,int userId)
        {
            IEnumerable<GameSessionsUsersRole> gameSessions = await GetBySessionId(sessionId);
            GameSessionsUsersRole gsur = gameSessions.FirstOrDefault(gs => gs.SessionId.Equals(sessionId) && gs.UserId.Equals(userId));
            gsur.PlayerIngameStatusId = 3;
            _dataContext.GameSessionsUsersRoles?.Update(gsur);
            await _dataContext.SaveChangesAsync();
            Role role = await _roleService.Get(gsur.RoleId);
            return $"{role.Role1} is dead!";
        }
    }
}

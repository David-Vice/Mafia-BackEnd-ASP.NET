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
    public class RoleService : IRoleService
    {
        private readonly IDataContext _dataContext;


        public RoleService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Add(Role role)
        {
            if (role != null)
            {
                role.BotResponses = _dataContext.BotResponses.Where(x=>x.RoleId == role.Id).ToList();
                role.GameSessionsUsersRoles = _dataContext.GameSessionsUsersRoles.Where(x => x.RoleId == role.Id).ToList();
                _dataContext.Roles?.Add(role);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var roleToDelete = await _dataContext.Roles.FindAsync(id);
            if (roleToDelete == null)
            {
                throw new NullReferenceException();
            }

            _dataContext.Roles.Remove(roleToDelete);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Role> Get(int id)
        {
            var role = await _dataContext.Roles.FindAsync(id);
            if (role == null)
            {
                throw new NullReferenceException();
            }
            return role;
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _dataContext.Roles.ToListAsync();
        }

        public async Task Update(int id, Role role)
        {
            Role roleToUpdate = await _dataContext.Roles.FindAsync(id);

            if (roleToUpdate == null)
            {
                throw new NullReferenceException();
            }

            roleToUpdate.Role1 = role.Role1;
            roleToUpdate.RoleDescription = role.RoleDescription;
            roleToUpdate.RolePhoto = role.RolePhoto;

            await _dataContext.SaveChangesAsync();
        }
    }

}
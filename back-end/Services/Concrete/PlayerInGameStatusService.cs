using back_end.Data;
using back_end.Models;
using back_end.Services.Abstract;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace back_end.Services.Concrete
{
    public class PlayerInGameStatusService : IPlayerInGameStatusService
    {
        private readonly IDataContext _dataContext;

        public PlayerInGameStatusService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Add(PlayerIngameStatus playerIngameStatus)
        {
            _dataContext.PlayerIngameStatuses?.Add(playerIngameStatus);
            await _dataContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var status = await _dataContext.PlayerIngameStatuses.FindAsync(id);
            if (status == null)
            {
                throw new NullReferenceException();
            }
            _dataContext?.PlayerIngameStatuses.Remove(status);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<PlayerIngameStatus> Get(int id)
        {
            var status = await _dataContext.PlayerIngameStatuses.FindAsync(id);
            if (status == null)
            {
                throw new NullReferenceException();
            }
            return status;
        }

        public async Task<IEnumerable<PlayerIngameStatus>> GetAll()
        {
            return await _dataContext.PlayerIngameStatuses.ToListAsync();
        }

        public async Task Update(int id, PlayerIngameStatus playerIngameStatus)
        {
            var status = await _dataContext.PlayerIngameStatuses.FindAsync(id);
            if (status == null)
            {
                throw new NullReferenceException(); 
            }
            status.Status = playerIngameStatus.Status;
            await _dataContext.SaveChangesAsync();
        }
    }
}

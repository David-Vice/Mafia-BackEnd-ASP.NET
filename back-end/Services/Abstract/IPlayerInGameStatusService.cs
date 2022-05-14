using back_end.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Services.Abstract
{
    public interface IPlayerInGameStatusService
    {
        Task<PlayerIngameStatus> Get(int id);
        Task<IEnumerable<PlayerIngameStatus>> GetAll();
        Task Add(PlayerIngameStatus playerIngameStatus);
        Task Update(int id, PlayerIngameStatus playerIngameStatus);
        Task Delete(int id);
    }
}

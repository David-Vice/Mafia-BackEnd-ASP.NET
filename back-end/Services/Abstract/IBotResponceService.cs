using back_end.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Services.Abstract
{
    public interface IBotResponceService
    {
        Task<BotResponse> Get(int id);
        Task<IEnumerable<BotResponse>> GetAll();
        Task<string> GetResponce(int roleId, int playerInGameStatusId);
        Task Add(BotResponse botResponse);
        Task Update(int id, BotResponse botResponse);
        Task Delete(int id);
    }
}

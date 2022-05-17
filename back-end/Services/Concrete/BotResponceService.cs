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
    public class BotResponceService : IBotResponceService
    {
        private readonly IDataContext _dataContext;
        public BotResponceService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Add(BotResponse botResponse)
        {
            if (botResponse != null)
            {
                botResponse.PlayerInGameStatus = _dataContext.PlayerIngameStatuses.Where(x=>x.Id == botResponse.PlayerInGameStatusId).FirstOrDefault();
                botResponse.Role = _dataContext.Roles.Where(x=>x.Id == botResponse.RoleId).FirstOrDefault();
                _dataContext.BotResponses?.Add(botResponse);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var responceToDelete = await _dataContext.BotResponses.FindAsync(id);
            if (responceToDelete == null)
            {
                throw new NullReferenceException();
            }
            _dataContext.BotResponses.Remove(responceToDelete);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<BotResponse> Get(int id)
        {
            var responce = await _dataContext.BotResponses.FindAsync(id);
            if (responce == null)
            {
                throw new NullReferenceException();
            }
            return responce;
        }

        public async Task<IEnumerable<BotResponse>> GetAll()
        {
            return await _dataContext.BotResponses.ToListAsync();
        }


        /// <summary>
        /// To get random answer from the bot according to the role and game status of the player
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="playerInGameStatusId"></param>
        /// <returns>Random responce of the bot</returns>
        public async Task<string> GetResponce(int roleId, int playerInGameStatusId)
        {
            IEnumerable<BotResponse> responces = await GetAll();  
            List<BotResponse> results = responces.Select(r=>r).Where(x=> x.RoleId == roleId && x.PlayerInGameStatusId == playerInGameStatusId).ToList();
            Random rand = new Random();
            return results[rand.Next(results.Count)].Response;
        }

        public async Task Update(int id, BotResponse botResponse)
        {
            var responce = await _dataContext.BotResponses.FindAsync(id);
            if (responce == null)
            {
                throw new NullReferenceException();
            }
            responce.RoleId = botResponse.RoleId;
            responce.PlayerInGameStatusId = botResponse.PlayerInGameStatusId;
            responce.Response = botResponse.Response;

            await _dataContext.SaveChangesAsync();
        }
    }
}

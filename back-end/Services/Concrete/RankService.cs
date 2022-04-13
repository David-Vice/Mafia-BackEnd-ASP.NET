using back_end.Data;
using back_end.Models;
using back_end.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services.Concrete
{
    public class RankService:IRankService
    {
        private readonly IDataContext _context;
        private readonly IUserService _userService;

        public RankService(IDataContext context,IUserService userService)
        {
            _context=context;
            _userService=userService;
        }
       
        public async Task<UserRank> GetUserRankByUserId(int userId)
        {
            //select userrank where 
            User user =await _userService.Get(userId);
            if (user == null)
            {
                throw new NullReferenceException();
            }
            UserRank userRank= await _context.UserRanks.FindAsync(user.UserRankId);
            if (userRank == null)
            {
                throw new NullReferenceException();
            }
            return userRank;
        }
    }
}

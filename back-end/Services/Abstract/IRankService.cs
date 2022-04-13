using back_end.Models;
using System.Threading.Tasks;

namespace back_end.Services.Abstract
{
    public interface IRankService
    {
        Task<UserRank> GetUserRankByUserId(int userId);
    }
}

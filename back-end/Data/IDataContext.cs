using back_end.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace back_end.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<BotResponse> BotResponses { get; set; }
        DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }
        DbSet<GameSessionsUsersRole> GameSessionsUsersRoles { get; set; }
        DbSet<PlayerIngameStatus> PlayerIngameStatuses { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Session> Sessions { get; set; }
        DbSet<UserRank> UserRanks { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}

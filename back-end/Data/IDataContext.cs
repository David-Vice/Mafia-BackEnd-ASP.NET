using back_end.Models;

namespace back_end.Data
{
    public interface IDataContext
    {
        DbSet<User>? Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}

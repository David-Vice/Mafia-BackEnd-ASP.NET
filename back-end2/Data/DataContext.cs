using back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end.Data
{
    public class DataContext:DbContext,IDataContext
    {
        //these 2 lines are needed when you add DbSet of new Model in database 
        //dotnet ef migrations add MigrationName
        //dotnet ef database update
        public DataContext(DbContextOptions<DataContext> options):base(options) { }
        public DbSet<User>? Users { get; set; }


    }
}

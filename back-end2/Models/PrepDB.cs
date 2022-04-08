namespace back_end.Models
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DataContext>());
            }
        }
        public static void SeedData(DataContext context)
        {
            Console.WriteLine("Appling Migrations..");
            context.Database.Migrate();

            if (!context.Users.Any())
            {
                Console.WriteLine("Adding data - seeding");
                context.Users.AddRange(
                    new User()
                    {

                        Name = "User1",
                        Surname = "Userr1",
                        Email = "a@mail.com",
                        Username = "User1"

                    }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Already have data - not seeding");
            }
        }
    }
}

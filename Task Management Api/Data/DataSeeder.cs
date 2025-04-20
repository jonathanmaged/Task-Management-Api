using Task_Management_Api.Models;

namespace Task_Management_Api.Data
{
    public static class DataSeeder
    {

        public static void SeedDatabase(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Id = 1, Name = "John" },
                    new User { Id = 2, Name = "Maged" },
                    new User { Id = 3, Name = "Youssef" }
                );
            }


            context.SaveChanges();
        }

    }
}

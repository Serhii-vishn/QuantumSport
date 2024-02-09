namespace QuantumSport.API.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.Users.Any())
            {
                await context.Users.AddRangeAsync(
                    new List<UserEntity>()
                    {
                        new UserEntity() { Name = "Billy Herrington", Phone = "+1234567890" },
                        new UserEntity() { Name = "Arnold Schwarzenegger", Phone = "+1000100011" },
                        new UserEntity() { Name = "Ronnie Coleman", Phone = "+1212121212" },
                        new UserEntity() { Name = "Vasyl Virastyuk", Phone = "+0987654321" },
                    });

                await context.SaveChangesAsync();
            }
        }
    }
}

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
                        new UserEntity() { Name = "Billy Herrington", Phone = "380994782390" },
                        new UserEntity() { Name = "Arnold Schwarzenegger", Phone = "380663232343" },
                        new UserEntity() { Name = "Ronnie Coleman", Phone = "380983454875" },
                        new UserEntity() { Name = "Vasyl Virastyuk", Phone = "380992301223" },
                    });

                await context.SaveChangesAsync();
            }
        }
    }
}

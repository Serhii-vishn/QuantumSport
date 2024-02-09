namespace QuantumSport.API.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(DbContext context)
        {
            await context.Database.EnsureCreatedAsync();
        }
    }
}

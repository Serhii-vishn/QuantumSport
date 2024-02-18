namespace QuantumSport.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CoachEntity> Coaches { get; set; }
        public DbSet<NutritionistEntity> Nutritionists { get; set; }
        public DbSet<SportSectionEntity> SportSections { get; set; }
        public DbSet<IndividualMealPlanOrderEntity> IndividualMealPlanOrders { get; set; }
        public DbSet<IndividualTrainingProgramOrderEntity> IndividualTrainingProgramOrders { get; set; }
        public DbSet<CoachSportSectionEntity> CoachSportSections { get; set; }
        public DbSet<TrainingEntity> Trainings { get; set; }
        public DbSet<UserTrainingRecordEntity> UserTrainingRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}

namespace QuantumSport.API.Data.EntityConfigurations
{
    public class IndividualMealPlanOrderEntityTypeConfiguration : IEntityTypeConfiguration<IndividualMealPlanOrderEntity>
    {
        public void Configure(EntityTypeBuilder<IndividualMealPlanOrderEntity> builder)
        {
            builder.ToTable("IndividualMealPlanOrder");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(o => o.UserId)
                .IsRequired();

            builder.Property(o => o.NutritionistId)
                .IsRequired();

            builder.HasIndex(o => new { o.UserId, o.NutritionistId })
                .IsUnique();

            builder.HasOne(o => o.User)
                .WithMany(u => u.IndividualMealPlanOrders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Nutritionist)
                .WithMany(n => n.IndividualMealPlanOrders)
                .HasForeignKey(o => o.NutritionistId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

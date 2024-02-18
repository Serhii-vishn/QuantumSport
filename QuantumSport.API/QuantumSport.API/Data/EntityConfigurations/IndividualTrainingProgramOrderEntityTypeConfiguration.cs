namespace QuantumSport.API.Data.EntityConfigurations
{
    public class IndividualTrainingProgramOrderEntityTypeConfiguration : IEntityTypeConfiguration<IndividualTrainingProgramOrderEntity>
    {
        public void Configure(EntityTypeBuilder<IndividualTrainingProgramOrderEntity> builder)
        {
            builder.ToTable("IndividualTrainingProgramOrder");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(o => o.UserId)
                .IsRequired();

            builder.Property(o => o.CoachId)
                .IsRequired();

            builder.HasIndex(o => new { o.UserId, o.CoachId })
                .IsUnique();

            builder.HasOne(o => o.User)
                .WithMany(u => u.IndividualTrainingProgramOrders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Coach)
                .WithMany(c => c.IndividualTrainingProgramOrders)
                .HasForeignKey(o => o.CoachId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

namespace QuantumSport.API.Data.EntityConfigurations
{
    public class TrainingEntityTypeConfiguration : IEntityTypeConfiguration<TrainingEntity>
    {
        public void Configure(EntityTypeBuilder<TrainingEntity> builder)
        {
            builder.ToTable("Training");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(t => t.StartDate)
                .IsRequired();

            builder.Property(t => t.MaxAmount)
                .IsRequired();

            builder.Property(t => t.ActualAmount)
                .IsRequired();

            builder.HasIndex(t => new { t.CoachSportSectionId, t.StartDate })
                .IsUnique();

            builder.HasOne(t => t.CoachSportSection)
                .WithMany(cs => cs.Trainings)
                .HasForeignKey(t => t.CoachSportSectionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

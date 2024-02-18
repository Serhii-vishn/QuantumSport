namespace QuantumSport.API.Data.EntityConfigurations
{
    public class UserTrainingRecordEntityTypeConfiguration : IEntityTypeConfiguration<UserTrainingRecordEntity>
    {
        public void Configure(EntityTypeBuilder<UserTrainingRecordEntity> builder)
        {
            builder.ToTable("UserTrainingRecord");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(r => r.TrainingDate)
                .IsRequired();

            builder.HasIndex(r => new { r.UserId, r.TrainingDate })
                .IsUnique();

            builder.HasOne(r => r.User)
                .WithMany(u => u.Trainings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Training)
                .WithMany(t => t.Users)
                .HasForeignKey(r => r.TrainingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

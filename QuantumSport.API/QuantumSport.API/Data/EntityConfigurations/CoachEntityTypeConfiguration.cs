namespace QuantumSport.API.Data.EntityConfigurations
{
    public class CoachEntityTypeConfiguration : IEntityTypeConfiguration<CoachEntity>
    {
        public void Configure(EntityTypeBuilder<CoachEntity> builder)
        {
            builder.ToTable("Coach");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(55);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(55);

            builder.Property(c => c.BirthDate)
                .IsRequired();

            builder.Property(c => c.Education)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.Achievement)
                .IsRequired()
                .HasMaxLength(555);

            builder.Property(c => c.PictureFileName)
                .IsRequired(false)
                .HasMaxLength(55);
        }
    }
}

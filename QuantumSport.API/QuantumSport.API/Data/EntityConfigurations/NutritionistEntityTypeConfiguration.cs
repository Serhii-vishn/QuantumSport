namespace QuantumSport.API.Data.EntityConfigurations
{
    public class NutritionistEntityTypeConfiguration : IEntityTypeConfiguration<NutritionistEntity>
    {
        public void Configure(EntityTypeBuilder<NutritionistEntity> builder)
        {
            builder.ToTable("Nutritionist");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(n => n.FirstName)
                .IsRequired()
                .HasMaxLength(55);

            builder.Property(n => n.LastName)
                .IsRequired()
                .HasMaxLength(55);

            builder.Property(n => n.BirthDate)
                .IsRequired();

            builder.Property(n => n.Education)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(n => n.Spezialization)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(n => n.PictureFileName)
                .IsRequired(false)
                .HasMaxLength(55);
        }
    }
}

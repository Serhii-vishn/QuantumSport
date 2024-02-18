namespace QuantumSport.API.Data.EntityConfigurations
{
    public class SportSectionEntityTypeConfiguration : IEntityTypeConfiguration<SportSectionEntity>
    {
        public void Configure(EntityTypeBuilder<SportSectionEntity> builder)
        {
            builder.ToTable("SportSection");

            builder.HasKey(s => s.Id);

            builder.Property(u => u.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(55);

            builder.Property(u => u.Description)
                .IsRequired()
                .HasMaxLength(555);

            builder.Property(c => c.PictureFileName)
                .IsRequired(false)
                .HasMaxLength(55);
        }
    }
}

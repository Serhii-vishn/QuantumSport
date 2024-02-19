namespace QuantumSport.API.Data.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(55);

            builder.Property(u => u.Phone)
                .HasMaxLength(13)
                .IsFixedLength()
                .IsRequired();

            builder.HasIndex(u => u.Phone)
                .IsUnique();
        }
    }
}

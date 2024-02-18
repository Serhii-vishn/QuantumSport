namespace QuantumSport.API.Data.EntityConfigurations
{
    public class CoachSportSectionEntityTypeConfiguration : IEntityTypeConfiguration<CoachSportSectionEntity>
    {
        public void Configure(EntityTypeBuilder<CoachSportSectionEntity> builder)
        {
            builder.ToTable("CoachSportSection");

            builder.HasKey(cs => cs.Id);

            builder.Property(cs => cs.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(cs => cs.CoachId)
                .IsRequired();

            builder.Property(cs => cs.SportSectionId)
                .IsRequired();

            builder.HasIndex(cs => new { cs.CoachId, cs.SportSectionId })
                .IsUnique();

            builder.HasOne(cs => cs.Coach)
                .WithMany(c => c.SportSections)
                .HasForeignKey(cs => cs.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cs => cs.SportSection)
                .WithMany(s => s.Coaches)
                .HasForeignKey(cs => cs.SportSectionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

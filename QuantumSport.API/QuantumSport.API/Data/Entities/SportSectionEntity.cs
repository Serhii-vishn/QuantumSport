namespace QuantumSport.API.Data.Entities
{
    public class SportSectionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? PictureFileName { get; set; }

        public IList<CoachSportSectionEntity> Coaches { get; set; } = new List<CoachSportSectionEntity>();
    }
}

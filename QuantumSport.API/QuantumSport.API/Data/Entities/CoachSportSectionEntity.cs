namespace QuantumSport.API.Data.Entities
{
    public class CoachSportSectionEntity
    {
        public int Id { get; set; }

        public int CoachId { get; set; }
        public CoachEntity Coach { get; set; } = null!;

        public int SportSectionId { get; set; }
        public SportSectionEntity SportSection { get; set; } = null!;

        public IList<TrainingEntity> Trainings { get; set; } = new List<TrainingEntity>();
    }
}

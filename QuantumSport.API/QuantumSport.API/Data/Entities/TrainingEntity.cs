namespace QuantumSport.API.Data.Entities
{
    public class TrainingEntity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int MaxAmount { get; set; }
        public int ActualAmount { get; set; }

        public int CoachSportSectionId { get; set; }
        public CoachSportSectionEntity CoachSportSection { get; set; } = null!;

        public IList<UserTrainingRecordEntity> Users { get; set; } = new List<UserTrainingRecordEntity>();
    }
}

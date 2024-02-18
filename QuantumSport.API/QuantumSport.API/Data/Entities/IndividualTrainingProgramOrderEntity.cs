namespace QuantumSport.API.Data.Entities
{
    public class IndividualTrainingProgramOrderEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public int CoachId { get; set; }
        public CoachEntity Coach { get; set; } = null!;
    }
}

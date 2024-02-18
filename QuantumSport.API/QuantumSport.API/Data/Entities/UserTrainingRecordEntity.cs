namespace QuantumSport.API.Data.Entities
{
    public class UserTrainingRecordEntity
    {
        public int Id { get; set; }
        public DateTime TrainingDate { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public int TrainingId { get; set; }
        public TrainingEntity Training { get; set; } = null!;
    }
}

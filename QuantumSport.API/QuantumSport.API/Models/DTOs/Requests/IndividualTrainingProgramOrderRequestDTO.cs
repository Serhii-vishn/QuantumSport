namespace QuantumSport.API.Models.DTOs
{
    public class IndividualTrainingProgramOrderRequestDTO
    {
        public string UserName { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        public int CoachId { get; set; }
    }
}

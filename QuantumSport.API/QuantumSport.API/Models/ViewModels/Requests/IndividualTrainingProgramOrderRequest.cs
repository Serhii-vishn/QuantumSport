namespace QuantumSport.API.Models.ViewModels
{
    public class IndividualTrainingProgramOrderRequest
    {
        public string UserName { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        public int CoachId { get; set; }
    }
}

namespace QuantumSport.API.Services.Interfaces
{
    public interface IIndividualTrainingProgramOrderService
    {
        Task<int> AddAsync(IndividualTrainingProgramOrderRequestDTO trainingProgramOrderDTO);
    }
}

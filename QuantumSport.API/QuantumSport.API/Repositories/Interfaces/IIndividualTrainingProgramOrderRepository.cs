namespace QuantumSport.API.Repositories.Interfaces
{
    public interface IIndividualTrainingProgramOrderRepository
    {
        Task<int> AddAsync(IndividualTrainingProgramOrderEntity trainingProgramOrder);
    }
}

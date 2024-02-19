namespace QuantumSport.API.Repositories.Interfaces
{
    public interface ICoachRepository
    {
        Task<CoachEntity?> GetAsync(int id);
        Task<IList<CoachEntity>> ListAsync();
    }
}

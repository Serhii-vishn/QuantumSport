namespace QuantumSport.API.Services.Interfaces
{
    public interface ICoachService
    {
        Task<CoachDTO> GetAsync(int id);
        Task<IList<CoachDTO>> ListAsync();
    }
}

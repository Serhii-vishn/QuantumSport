namespace QuantumSport.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetAsync(int id);
        Task<IList<UserDTO>> ListAsync();
        Task<int> AddAsync(UserDTO user);
        Task<int> UpdateAsync(UserDTO user);
        Task<int> DeleteAsync(int id);
    }
}

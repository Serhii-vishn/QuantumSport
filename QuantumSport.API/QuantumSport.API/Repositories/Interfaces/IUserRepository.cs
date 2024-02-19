namespace QuantumSport.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetAsync(int id);
        Task<UserEntity?> GetAsync(string phoneNumber);
        Task<IList<UserEntity>> ListAsync();
        Task<int> AddAsync(UserEntity user);
        Task<int> UpdateAsync(UserEntity user);
        Task<int> DeleteAsync(int id);
    }
}

namespace QuantumSport.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> GetAsync(int id);
        Task<IList<UserEntity>> ListAsync();
        Task<int> AddAsync(UserEntity user);
        Task<int> UpdateAsync(UserEntity user);
        Task<int> DeleteAsync(int id);
    }
}

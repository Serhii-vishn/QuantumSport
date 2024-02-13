using System.Text.RegularExpressions;

namespace QuantumSport.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetAsync(int id)
        {
            var data = await _userRepository.GetAsync(id);

            if (data == null)
            {
                throw new UserNotFoundException($"User with id = {id} does not exist");
            }

            return _mapper.Map<UserDTO>(data);
        }

        public async Task<UserDTO> GetAsync(string phoneNumber)
        {
            var data = await _userRepository.GetAsync(phoneNumber);

            if (data == null)
            {
                throw new UserNotFoundException($"User with phone = {phoneNumber} does not exist");
            }

            return _mapper.Map<UserDTO>(data);
        }

        public async Task<IList<UserDTO>> ListAsync()
        {
            var data = await _userRepository.ListAsync();
            return data.Select(d => _mapper.Map<UserDTO>(d)).ToList();
        }

        public async Task<int> AddAsync(UserDTO user)
        {
            ValidateUser(user);
            user.Id = default;
            return await _userRepository.AddAsync(_mapper.Map<UserEntity>(user));
        }

        public async Task<int> UpdateAsync(UserDTO user)
        {
            await GetAsync(user.Id);
            ValidateUser(user);
            return await _userRepository.UpdateAsync(_mapper.Map<UserEntity>(user));
        }

        public async Task<int> DeleteAsync(int id)
        {
            await GetAsync(id);
            return await _userRepository.DeleteAsync(id);
        }

        private static void ValidateUser(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User is empty");
            }

            Regex wordPattern = new Regex("^[a-zA-Z- ]+$");
            Regex phonePattern = new Regex("^[+][0-9]{10}$");

            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new ArgumentException(nameof(user.Name), "User name is empty");
            }
            else
            {
                user.Name = user.Name.Trim();

                if (!wordPattern.IsMatch(user.Name) || user.Name.Length < 3 || user.Name.Length > 255)
                {
                    throw new ArgumentException(nameof(user.Name), "User name is invalid");
                }
            }

            if (string.IsNullOrWhiteSpace(user.Phone))
            {
                throw new ArgumentNullException(nameof(user.Phone), "Phone is empty");
            }
            else
            {
                user.Phone = user.Phone.Trim();

                if (!user.Phone.StartsWith('+'))
                {
                    user.Phone = user.Phone.Insert(0, "+");
                }

                if (!phonePattern.IsMatch(user.Phone))
                {
                    throw new ArgumentException(nameof(user.Phone), "Phone is invalid");
                }
            }
        }
    }
}

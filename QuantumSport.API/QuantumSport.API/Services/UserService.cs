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
            if (id <= 0)
            {
                throw new ArgumentException("Invalid user id");
            }

            var data = await _userRepository.GetAsync(id);

            if (data == null)
            {
                throw new UserNotFoundException($"User with id = {id} does not exist");
            }

            return _mapper.Map<UserDTO>(data);
        }

        public async Task<UserDTO> GetAsync(string phoneNumber)
        {
            ValidateUserPhone(phoneNumber);

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

            var data = await _userRepository.GetAsync(user.Phone);
            if (data != null)
            {
                throw new ArgumentException($"User with phone = {user.Phone} already exists");
            }

            user.Id = default;

            return await _userRepository.AddAsync(_mapper.Map<UserEntity>(user));
        }

        public async Task<int> UpdateAsync(UserDTO user)
        {
            ValidateUser(user);

            var data = await _userRepository.GetAsync(user.Phone);
            if (data != null)
            {
                throw new ArgumentException($"User with phone = {user.Phone} already exists");
            }

            await GetAsync(user.Id);

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

            ValidateUserName(user.Name);

            ValidateUserPhone(user.Phone);
        }

        private static void ValidateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName), "User name is empty");
            }
            else
            {
                userName = userName.Trim();

                if (userName.Length < 3)
                {
                    throw new ArgumentException(nameof(userName), "User name must be at least 3 characters long");
                }

                if (userName.Length > 55)
                {
                    throw new ArgumentException(nameof(userName), "User name must be maximum of 55 characters");
                }

                Regex englishWordPattern = new ("^[a-zA-Z -]+$");
                Regex ukrainianWordPattern = new ("^[АаБбВвГгҐґДдЕеЄєЖжЗзИиІіЇїЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщьЮюЯя -]+$");

                if (!englishWordPattern.IsMatch(userName))
                {
                    if (!ukrainianWordPattern.IsMatch(userName))
                    {
                        throw new ArgumentException(nameof(userName), "User name must consist of english or ukrainian letters only");
                    }
                }
            }
        }

        private static void ValidateUserPhone(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentNullException(nameof(phoneNumber), "Phone is empty");
            }
            else
            {
                phoneNumber = phoneNumber.Trim();

                const string ukrainianPhoneNumberPattern = @"^\+380\d{9}$";

                if (!Regex.IsMatch(phoneNumber, ukrainianPhoneNumberPattern))
                {
                    throw new ArgumentException(nameof(phoneNumber), "Phone is invalid");
                }
            }
        }
    }
}

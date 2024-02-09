namespace QuantumSport.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;

        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly UserEntity _fakeUserEntity = new UserEntity()
        {
            Id = 1,
            Name = "name",
            Phone = "+1234567890",
        };

        private readonly UserDTO _fakeUserDTO = new UserDTO()
        {
            Id = 1,
            Name = "name",
            Phone = "+1234567890",
        };

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();

            _userService = new UserService(_userRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task ListAsync_WithoutConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var sqlEx = MakeSqlException();
            _userRepository.Setup(s => s.ListAsync()).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _userService.ListAsync());
        }

        [Fact]
        public async Task ListAsync_ReturnsListUserDTO()
        {
            // Arrange
            var fakeUserEntityList = new List<UserEntity>() { _fakeUserEntity };
            _userRepository.Setup(s => s.ListAsync()).ReturnsAsync(fakeUserEntityList);

            _mapper.Setup(s => s.Map<UserDTO>(
                It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);

            // Act
            var result = await _userService.ListAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveSameCount(fakeUserEntityList);
            result.First().Should().NotBeNull();
            result.First().Should().BeOfType<UserDTO>();
            result.First().Should().Be(_fakeUserDTO);
        }

        [Fact]
        public async Task GetAsync_WithoutConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var sqlEx = MakeSqlException();
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _userService.GetAsync(_fakeUserDTO.Id));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetAsync_PassingInvalidId_ThrowsUserNotFoundException(int id)
        {
            // Arrange
            UserEntity nullUserEntity = null!;
            _userRepository.Setup(s => s.GetAsync(id)).ReturnsAsync(nullUserEntity);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.GetAsync(id));
        }

        [Fact]
        public async Task GetAsync_ReturnsUserDTO()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);

            _mapper.Setup(s => s.Map<UserDTO>(
                It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);

            // Act
            var result = await _userService.GetAsync(_fakeUserDTO.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserDTO>();
            result.Should().Be(_fakeUserDTO);
        }

        private SqlException MakeSqlException()
        {
            SqlException exception = null!;

            try
            {
                SqlConnection conn = new SqlConnection("Connection Timeout=1");
                conn.Open();
            }
            catch (SqlException ex)
            {
                exception = ex;
            }

            return exception;
        }
    }
}

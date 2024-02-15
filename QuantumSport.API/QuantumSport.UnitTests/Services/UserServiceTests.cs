using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Routing;
using Moq;

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

        [Fact]
        public async Task AddAsync_AddsUserSuccessfully()
        {
            // Arrange
            _mapper.Setup(m => m.Map<UserEntity>(_fakeUserDTO)).Returns(_fakeUserEntity);
            _userRepository.Setup(r => r.AddAsync(_fakeUserEntity)).ReturnsAsync(1);

            // Act
            var result = await _userService.AddAsync(_fakeUserDTO);

            // Assert
            result.Should().Be(_fakeUserDTO.Id);
        }

        [Fact]
        public async Task AddAsync_InvalidUserDTO_ThrowsArgumentException()
        {
            // Arrange
            var invalidUserDTO = new UserDTO(); // Create an invalid UserDTO object

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(invalidUserDTO));
        }

        [Fact]
        public async Task AddAsync_ErrorAddingUser_ThrowsException()
        {
            // Arrange
            _mapper.Setup(m => m.Map<UserEntity>(_fakeUserDTO)).Returns(_fakeUserEntity);
            _userRepository.Setup(r => r.AddAsync(_fakeUserEntity)).ThrowsAsync(new Exception("Error adding user"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.AddAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_UpdateUserSuccessfull()
        {
            // Arrange
            _mapper.Setup(m => m.Map<UserEntity>(_fakeUserDTO)).Returns(_fakeUserEntity);
            _userRepository.Setup(r => r.UpdateAsync(_fakeUserEntity)).ReturnsAsync(1);

            // Act
            var result = await _userService.UpdateAsync(_fakeUserDTO);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task UpdateAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            _userRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(() => new UserEntity());

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_InvalidUserDTO_ThrowsArgumentException()
        {
            // Arrange
            var invalidUserDTO = new UserDTO(); // Create an invalid UserDTO object

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(invalidUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_ErrorUpdatingUser_ThrowsException()
        {
            // Arrange
            _mapper.Setup(m => m.Map<UserEntity>(_fakeUserDTO)).Returns(_fakeUserEntity);
            _userRepository.Setup(r => r.UpdateAsync(_fakeUserEntity)).ThrowsAsync(new Exception("Error updating user"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task DeleteAsync_DeleteUserSuccessfully()
        {
            // Arrange
            _userRepository.Setup(r => r.DeleteAsync(_fakeUserEntity.Id)).ReturnsAsync(1);

            // Act
            var result = await _userService.DeleteAsync(_fakeUserEntity.Id);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task DeleteAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            _userRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(() => new UserEntity());

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(_fakeUserEntity.Id));
        }

        [Fact]
        public async Task DeleteAsync_ErrorDeletingUser_ThrowsException()
        {
            // Arrange
            _userRepository.Setup(r => r.DeleteAsync(_fakeUserEntity.Id)).ThrowsAsync(new Exception("Error deleting user"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.DeleteAsync(_fakeUserEntity.Id));
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

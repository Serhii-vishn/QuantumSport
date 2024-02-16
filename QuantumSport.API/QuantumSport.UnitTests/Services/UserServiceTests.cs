using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
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
        public async Task AddAsync_NoConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var sqlEx = MakeSqlException();
            _mapper.Setup(s => s.Map<UserEntity>(
                It.Is<UserDTO>(i => i.Equals(_fakeUserDTO)))).Returns(_fakeUserEntity);
            _userRepository.Setup(s => s.AddAsync(It.IsAny<UserEntity>())).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _userService.AddAsync(_fakeUserDTO));
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
            result.Should().Be(_fakeUserEntity.Id);
        }

        [Fact]
        public async Task AddAsync_WhenDuplicateUserId_ShouldPreventRepositoryAddCall()
        {
            // Arrange
            _userRepository.Setup(repo => repo.AddAsync(It.IsAny<UserEntity>())).Verifiable();

            // Act
            await _userService.AddAsync(_fakeUserDTO);

            // Assert
            _userRepository.Verify(repo => repo.AddAsync(It.IsAny<UserEntity>()), Times.Once());
        }

        [Fact]
        public async Task AddAsync_WhenNameExceedsMaxLength_ShouldThrowArgumentException()
        {
            // Arrange
            var userWithLongName = new UserDTO()
            {
                Id = 0,
                Name = new string('x', 256), // Creating a name with more than 255 characters
                Phone = "+1234567890",
            };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(userWithLongName));
        }

        [Fact]
        public async Task AddAsync_WhenNameContainsNonAlphabeticCharacters_ShouldThrowException()
        {
            // Arrange
            var userWithNonAlphabeticCharacters = new UserDTO()
            {
                Id = 0,
                Name = "name$%123", // Name containing non-alphabetic characters
                Phone = "+1234567890",
            };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(userWithNonAlphabeticCharacters));

            // Additional check with regular expression
            var regex = new Regex(@"\P{L}");
            Assert.Matches(regex, userWithNonAlphabeticCharacters.Name);
        }

        [Fact]
        public async Task AddAsync_NameShouldContainAtLeastThreeCharacters()
        {
            // Arrange
            _mapper.Setup(s => s.Map<UserDTO>(
                It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);
            _fakeUserDTO.Name = "Aa";

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task AddAsync_WhenAnyFieldIsEmpty_ShouldThrowException()
        {
            // Arrange
            var userWithEmptyFields = new UserDTO()
            {
                Id = 0,
                Name = string.Empty, // Empty name
                Phone = string.Empty, // Empty phone
            };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(userWithEmptyFields));
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
        public async Task AddAsync_UserPhoneIsInvalid_ThrowsArgumentException()
        {
            // Arrange
            _mapper.Setup(s => s.Map<UserDTO>(
                It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);
            _fakeUserDTO.Phone = "10 20 abc";

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task DeleteAsync_NoConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var userId = 1;
            var sqlEx = MakeSqlException();
            _userRepository.Setup(s => s.GetAsync(userId)).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _userService.DeleteAsync(userId));
        }

        [Fact]
        public async Task UpdateAsync_UpdateUserSuccessfull()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _mapper.Setup(m => m.Map<UserEntity>(_fakeUserDTO)).Returns(_fakeUserEntity);
            _userRepository.Setup(r => r.UpdateAsync(_fakeUserEntity)).ReturnsAsync(_fakeUserEntity.Id);

            // Act
            var result = await _userService.UpdateAsync(_fakeUserDTO);

            // Assert
            result.Should().NotBe(null);
            result.Should().Be(_fakeUserEntity.Id);
        }

        [Fact]
        public async Task UpdateAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            UserEntity nullUserEntity = null!;
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(nullUserEntity);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_InvalidUserDTO_ThrowsArgumentException()
        {
            // Arrange
            var invalidUserDTO = new UserDTO(); // Create an invalid UserDTO object

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateAsync(invalidUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_ErrorUpdatingUser_ThrowsException()
        {
            // Arrange
            _mapper.Setup(m => m.Map<UserEntity>(_fakeUserDTO)).Returns(_fakeUserEntity);
            _userRepository.Setup(r => r.UpdateAsync(_fakeUserEntity)).ThrowsAsync(new Exception("Error updating user"));

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_WhenNameExceedsMaxLength_ShouldThrowArgumentException()
        {
            // Arrange
            var userWithLongName = new UserDTO()
            {
                Id = _fakeUserDTO.Id,
                Name = new string('x', 256), // Creating a name with more than 255 characters
                Phone = "+1234567890",
            };

            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(_fakeUserEntity);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(userWithLongName));
        }

        [Fact]
        public async Task UpdateAsync_WhenNameContainsNonAlphabeticCharacters_ShouldThrowException()
        {
            // Arrange
            var userWithNonAlphabeticCharacters = new UserDTO()
            {
                Id = 0,
                Name = "name$%123", // Name containing non-alphabetic characters
                Phone = "+1234567890",
            };

            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(_fakeUserEntity);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(userWithNonAlphabeticCharacters));

            // Additional check with regular expression
            var regex = new Regex(@"\P{L}");
            Assert.Matches(regex, userWithNonAlphabeticCharacters.Name);
        }

        [Fact]
        public async Task UpdateAsync_WhenAnyFieldIsEmpty_ShouldThrowException()
        {
            // Arrange
            var userWithEmptyFields = new UserDTO()
            {
                Id = _fakeUserDTO.Id,
                Name = string.Empty, // Empty name
                Phone = string.Empty, // Empty phone
            };

            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(_fakeUserEntity);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(userWithEmptyFields));
        }

        [Fact]
        public async Task UpdateAsync_NameShouldContainAtLeastThreeCharacters()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _mapper.Setup(s => s.Map<UserDTO>(
                It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);
            _fakeUserDTO.Name = "Aa";

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_UserPhoneIsInvalid_ThrowsArgumentException()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _mapper.Setup(s => s.Map<UserDTO>(
                It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);
            _fakeUserDTO.Phone = "10 20 abc";

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_NoConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var userId = 1;
            var sqlEx = MakeSqlException();
            _userRepository.Setup(s => s.GetAsync(userId)).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task DeleteAsync_DeleteUserSuccessfully()
        {
            // Arrange
            _userRepository.Setup(r => r.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _userRepository.Setup(s => s.DeleteAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity.Id);

            // Act
            var result = await _userService.DeleteAsync(_fakeUserEntity.Id);

            // Assert
            result.Should().Be(_fakeUserEntity.Id);
        }

        [Fact]
        public async Task DeleteAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            UserEntity nullUserEntity = null!;
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(nullUserEntity);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(_fakeUserEntity.Id));
        }

        [Fact]
        public async Task DeleteAsync_ErrorDeletingUser_ThrowsException()
        {
            // Arrange
            _userRepository.Setup(r => r.DeleteAsync(_fakeUserEntity.Id)).ThrowsAsync(new Exception("Error deleting user"));

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(_fakeUserEntity.Id));
        }

        [Fact]
        public async Task DeleteAsync_WhenNameExceedsMaxLength_ShouldThrowArgumentException()
        {
            // Arrange
            var userWithLongName = new UserDTO()
            {
                Id = 1,
                Name = new string('x', 256), // Creating a name with more than 255 characters
                Phone = "+1234567890",
            };

            // Act + Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(userWithLongName.Id));
        }

        [Fact]
        public async Task DeleteAsync_WhenNameContainsNonAlphabeticCharacters_ShouldThrowException()
        {
            // Arrange
            var userWithNonAlphabeticCharacters = new UserDTO()
            {
                Id = 1,
                Name = "name$%123", // Name containing non-alphabetic characters
                Phone = "+1234567890",
            };

            // Act + Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(userWithNonAlphabeticCharacters.Id));

            // Additional check with regular expression
            var regex = new Regex(@"\P{L}");
            Assert.Matches(regex, userWithNonAlphabeticCharacters.Name);
        }

        [Fact]
        public async Task DeleteAsync_WhenAnyFieldIsEmpty_ShouldThrowException()
        {
            // Arrange
            var userWithEmptyFields = new UserDTO()
            {
                Id = 1,
                Name = string.Empty, // Empty name
                Phone = string.Empty, // Empty phone
            };

            // Act + Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(userWithEmptyFields.Id));
        }

        [Fact]

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

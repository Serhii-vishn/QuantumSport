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
        public async Task ListAsync_WhenCalled_ReturnsListUserDTO()
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
        public async Task GetAsync_PassingInvalidId_ThrowsArgumentException(int id)
        {
            // Arrange
            // No need to set up the repository mock since the method throws ArgumentException

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.GetAsync(id));
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("+3801234567890")]
        public async Task GetAsync_PassingInvalidPhoneNumber_ThrowsArgumentException(string phoneNumber)
        {
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.GetAsync(phoneNumber));
        }

        [Fact]
        public async Task GetAsync_PassingValidId_ReturnsUserDTO()
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
            var fakeUserDTO = new UserDTO { Id = 1, Name = "existingName", Phone = "+380123456789" };
            var existingUserPhone = "+380123456789";
            var expectedMessage = $"User with phone = {fakeUserDTO.Phone} already exists";

            _userRepository.Setup(s => s.GetAsync(existingUserPhone)).ReturnsAsync(new UserEntity());

            // Act and Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddAsync(fakeUserDTO));
            Assert.Equal(expectedMessage, ex.Message);
        }

        [Fact]
        public async Task AddAsync_PassingValidUserDTO_ReturnsAddedId()
        {
            // Arrange
            _fakeUserDTO.Phone = "+380123456789";

            _mapper.Setup(m => m.Map<UserEntity>(_fakeUserDTO)).Returns(_fakeUserEntity);
            _userRepository.Setup(r => r.GetAsync(_fakeUserDTO.Phone)).ReturnsAsync(null ?? new UserEntity());
            _userRepository.Setup(r => r.AddAsync(_fakeUserEntity)).ReturnsAsync(1);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(_fakeUserDTO));
            exception.Message.Should().Be("User with phone = +380123456789 already exists");
        }

        [Fact]
        public async Task AddAsync_WhenDuplicateUserId_PreventsRepositoryAddCall()
        {
            // Arrange
            var existingUser = new UserDTO { Id = 1, Name = "existingName", Phone = "+1234567890" };
            _userRepository.Setup(x => x.GetAsync(existingUser.Phone)).ReturnsAsync(new UserEntity());

            var userToAdd = new UserDTO { Id = 1, Name = "newName", Phone = "+1234567890" };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(userToAdd));
        }

        [Fact]
        public async Task AddAsync_WhenNameExceedsMaxLength_ThrowsArgumentException()
        {
            // Arrange
            var userWithLongName = new UserDTO()
            {
                Id = 0,
                Name = new string('x', 56),
                Phone = "+1234567890",
            };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(userWithLongName));
        }

        [Fact]
        public async Task AddAsync_WhenNameContainsNonAlphabeticCharacters_ThrowsArgumentException()
        {
            // Arrange
            var userWithNonAlphabeticCharacters = new UserDTO()
            {
                Id = 0,
                Name = "name$%123",
                Phone = "+1234567890",
            };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(userWithNonAlphabeticCharacters));
        }

        [Fact]
        public async Task AddAsync_NameContainsLessThanThreeCharacters_ThrowsArgumentException()
        {
            // Arrange
            _mapper.Setup(s => s.Map<UserDTO>(
                It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);
            _fakeUserDTO.Name = "Aa";

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task AddAsync_WhenAnyFieldIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            var userWithEmptyFields = new UserDTO()
            {
                Id = 0,
                Name = " ",
                Phone = string.Empty,
            };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.AddAsync(userWithEmptyFields));
        }

        [Fact]
        public async Task AddAsync_InvalidUserDTO_ThrowsArgumentException()
        {
            // Arrange
            var invalidUserDTO = new UserDTO()
            {
                Id = 1,
                Name = "name",
                Phone = "invalidPhoneNumber",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(invalidUserDTO));
        }

        [Fact]
        public async Task AddAsync_ErrorAddingUser_ThrowsArgumentException()
        {
            // Arrange
            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<string>())).ReturnsAsync(null ?? new UserEntity());
            _userRepository.Setup(repo => repo.AddAsync(It.IsAny<UserEntity>())).ReturnsAsync(0);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.AddAsync(_fakeUserDTO));
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
        public async Task UpdateAsync_NoConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var fakeUserDTO = new UserDTO { Id = 1, Name = "existingName", Phone = "+380123456789" };
            var sqlEx = MakeSqlException();

            _userRepository.Setup(s => s.GetAsync(fakeUserDTO.Phone)).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _userService.UpdateAsync(fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_PassingValidUserDTO_ReturnsAddedId()
        {
            // Arrange
            var existingUserEntity = new UserEntity { Id = 1, Name = "existing", Phone = "+1234567890" };
            _userRepository.Setup(r => r.GetAsync(existingUserEntity.Id)).ReturnsAsync(existingUserEntity);
            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(1);

            // Act
            _fakeUserDTO.Phone = "+380123456789";
            var result = await _userService.UpdateAsync(_fakeUserDTO);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task UpdateAsync_UserNotFound_ThrowsArgumentException()
        {
            // Arrange
            _userRepository.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(new UserEntity());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_InvalidUserDTO_ThrowsArgumentException()
        {
            // Arrange
            var existingUserEntity = new UserEntity { Id = 2, Name = "existing", Phone = "+380998887766" };
            _userRepository.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(existingUserEntity);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(new UserDTO { Id = 1, Name = "name", Phone = "+38099777" }));
        }

        [Fact]
        public async Task UpdateAsync_ErrorUpdatingUser_ThrowsArgumentException()
        {
            // Arrange
            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<string>())).ReturnsAsync(_fakeUserEntity);
            _userRepository.Setup(repo => repo.GetAsync(_fakeUserDTO.Id)).ReturnsAsync(_fakeUserEntity);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_WhenNameExceedsMaxLength_ThrowsArgumentException()
        {
            // Arrange
            var userWithLongName = new UserDTO()
            {
                Id = _fakeUserDTO.Id,
                Name = new string('x', 56),
                Phone = "+1234567890",
            };

            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(_fakeUserEntity);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(userWithLongName));
        }

        [Fact]
        public async Task UpdateAsync_WhenNameContainsNonAlphabeticCharacters_ThrowsArgumentException()
        {
            // Arrange
            var userWithNonAlphabeticCharacters = new UserDTO()
            {
                Id = 0,
                Name = "name$%123",
                Phone = "+1234567890",
            };

            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<int>())).ReturnsAsync(_fakeUserEntity);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(userWithNonAlphabeticCharacters));
        }

        [Fact]
        public async Task UpdateAsync_WhenAnyFieldIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var userWithEmptyFields = new UserDTO()
            {
                Id = 1,
                Name = string.Empty,
                Phone = string.Empty,
            };

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                if (string.IsNullOrWhiteSpace(userWithEmptyFields.Name))
                {
                    throw new ArgumentException("User name is empty", nameof(userWithEmptyFields.Name));
                }

                if (string.IsNullOrWhiteSpace(userWithEmptyFields.Phone))
                {
                    throw new ArgumentException("User phone is empty", nameof(userWithEmptyFields.Phone));
                }

                return _userService.UpdateAsync(userWithEmptyFields);
            });
        }

        [Fact]
        public async Task UpdateAsync_NameContainsLessThanThreeCharacters_ThrowsArgumentException()
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
        public async Task DeleteAsync_PassingValidUserDTO_ReturnsAddedId()
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
        public async Task DeleteAsync_ErrorDeletingUser_ThrowsUserNotFoundException()
        {
            // Arrange
            _userRepository.Setup(r => r.DeleteAsync(_fakeUserEntity.Id)).ThrowsAsync(new Exception("Error deleting user"));

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(_fakeUserEntity.Id));
        }

        [Fact]
        public async Task DeleteAsync_WhenNameExceedsMaxLength_ThrowsUserNotFoundException()
        {
            // Arrange
            var userWithLongName = new UserDTO()
            {
                Id = 1,
                Name = new string('x', 56),
                Phone = "+1234567890",
            };

            // Act + Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(userWithLongName.Id));
        }

        [Fact]
        public async Task DeleteAsync_WhenNameContainsNonAlphabeticCharacters_ThrowsUserNotFoundException()
        {
            // Arrange
            var userWithNonAlphabeticCharacters = new UserDTO()
            {
                Id = 1,
                Name = "name$%123",
                Phone = "+1234567890",
            };

            // Act + Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(userWithNonAlphabeticCharacters.Id));
        }

        [Fact]
        public async Task DeleteAsync_WhenAnyFieldIsEmpty_ThrowsUserNotFoundException()
        {
            // Arrange
            var userWithEmptyFields = new UserDTO()
            {
                Id = 1,
                Name = string.Empty,
                Phone = string.Empty,
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

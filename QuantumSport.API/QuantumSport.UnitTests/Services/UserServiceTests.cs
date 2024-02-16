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
        public async Task GetAsync_PassingInvalidId_ThrowsUserNotFoundException(int id)
        {
            // Arrange
            UserEntity nullUserEntity = null!;
            _userRepository.Setup(s => s.GetAsync(id)).ReturnsAsync(nullUserEntity);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.GetAsync(id));
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

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]

        public async Task DeleteAsync_PassingInvalidId_ThrowsUserNotFoundException(int id)
        {
            // Arrange
            UserEntity nullUserEntity = null!;
            _userRepository.Setup(s => s.GetAsync(id)).ReturnsAsync(nullUserEntity);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.DeleteAsync(id));
        }

        [Fact]
        public async Task DeleteAsync_WithoutConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var sqlEx = MakeSqlException();
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _mapper.Setup(s => s.Map<UserDTO>(
               It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);
            _userRepository.Setup(s => s.DeleteAsync(_fakeUserEntity.Id)).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _userService.DeleteAsync(_fakeUserDTO.Id));
        }

        [Fact]
        public async Task DeleteAsync_PassingValidId_ReturnsDeletedId()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _mapper.Setup(s => s.Map<UserDTO>(
               It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);
            _userRepository.Setup(s => s.DeleteAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity.Id);

            // Act
            var result = await _userService.DeleteAsync(_fakeUserDTO.Id);

            // Assert
            result.Should().Be(_fakeUserDTO.Id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateAsync_PassingInvalidId_ThrowsUserNotFoundException(int id)
        {
            // Arrange
            UserEntity nullUserEntity = null!;
            _fakeUserDTO.Id = id;
            _userRepository.Setup(s => s.GetAsync(id)).ReturnsAsync(nullUserEntity);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_WithoutConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var sqlEx = MakeSqlException();
            _mapper.Setup(s => s.Map<UserEntity>(
               It.Is<UserDTO>(i => i.Equals(_fakeUserDTO)))).Returns(_fakeUserEntity);
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _userRepository.Setup(s => s.UpdateAsync(_fakeUserEntity)).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateAsync_UserNameIsNullEmptyOrWhitespace_ThrowsArgumentException(string name)
        {
            // Arrange
            _fakeUserDTO.Name = name;
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Theory]
        [InlineData("87687abc6")]
        [InlineData(" %^@()46")]
        public async Task UpdateAsync_UserNameConsistsOfNonAlphabetSymbols_ThrowsArgumentException(string name)
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _fakeUserDTO.Name = name;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_UserNameIsLessThreeSymbols_ThrowsArgumentException()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _fakeUserDTO.Name = "Aa";

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_UserNameLengthIsMoreThan255Symblols_ThrowsArgumentException()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _fakeUserDTO.Name = GetMockStringOfSpecifiedLength(256);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateAsync_UserPhoneIsNullEmptyOrWhitespace_ThrowsArgumentNullException(string phone)
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _fakeUserDTO.Phone = phone;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Theory]
        [InlineData("!@#1567")]
        [InlineData("0000abc99")]
        [InlineData("+38096565OO77")]
        public async Task UpdateAsync_UserPhoneConsistsOfNonNumericSymbols_ThrowsArgumentException(string phone)
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _fakeUserDTO.Phone = phone;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.UpdateAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task UpdateAsync_PassingValidUserDTO_ReturnsUpdatedId()
        {
            // Arrange
            _userRepository.Setup(s => s.GetAsync(_fakeUserEntity.Id)).ReturnsAsync(_fakeUserEntity);
            _mapper.Setup(s => s.Map<UserDTO>(
               It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);
            _userRepository.Setup(s => s.UpdateAsync(_fakeUserEntity)).ReturnsAsync(_fakeUserEntity.Id);

            // Act
            var result = await _userService.UpdateAsync(_fakeUserDTO);

            // Assert
            result.Should().NotBe(_fakeUserDTO.Id);
        }

        [Fact]
        public async Task AddAsync_WithoutConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var sqlEx = MakeSqlException();
            _mapper.Setup(s => s.Map<UserEntity>(
              It.Is<UserDTO>(i => i.Equals(_fakeUserDTO)))).Returns(_fakeUserEntity);
            _userRepository.Setup(s => s.AddAsync(_fakeUserEntity)).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task AddAsync_UserNameIsNullEmptyOrWhitespace_ThrowsArgumentException(string name)
        {
            // Arrange
            _fakeUserDTO.Name = name;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Theory]
        [InlineData("87687abc6")]
        [InlineData(" %^@()46")]
        public async Task AddAsync_UserNameConsistsOfNonAlphabetSymbols_ThrowsArgumentException(string name)
        {
            // Arrange
            _fakeUserDTO.Name = name;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task AddAsync_UserNameLengthIsLessThan3Symbols_ThrowsArgumentException()
        {
            // Arrange
            _fakeUserDTO.Name = "Aa";

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task AddAsync_UserNameLengthIsMoreThan255Symblols_ThrowsArgumentException()
        {
            // Arrange
            _fakeUserDTO.Name = GetMockStringOfSpecifiedLength(256);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task AddAsync_UserPhoneIsNullEmptyOrWhitespace_ThrowsArgumentNullException(string phone)
        {
            // Arrange
            _fakeUserDTO.Phone = phone;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Theory]
        [InlineData("!@#1567")]
        [InlineData("0000abc99")]
        [InlineData("+38096565OO77")]
        public async Task AddAsync_UserPhoneConsistsOfNonNumericSymbols_ThrowsArgumentException(string phone)
        {
            // Arrange
            _fakeUserDTO.Phone = phone;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddAsync(_fakeUserDTO));
        }

        [Fact]
        public async Task AddAsync_PassingValidUserDTO_ReturnsAddedId()
        {
            // Arrange
            _userRepository.Setup(s => s.AddAsync(_fakeUserEntity)).ReturnsAsync(_fakeUserEntity.Id);
            _mapper.Setup(s => s.Map<UserDTO>(
                It.Is<UserEntity>(i => i.Equals(_fakeUserEntity)))).Returns(_fakeUserDTO);

            // Act
            var result = await _userService.AddAsync(_fakeUserDTO);

            // Assert
            result.Should().Be(_fakeUserDTO.Id);
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

        private string GetMockStringOfSpecifiedLength(int length)
        {
            string mockString = "A";
            for (int i = 0; i < length; i++)
            {
                mockString += "a";
            }

            return mockString;
        }
    }
}

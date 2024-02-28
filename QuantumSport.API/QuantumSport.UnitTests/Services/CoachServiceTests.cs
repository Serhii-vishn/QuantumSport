using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace QuantumSport.UnitTests.Services
{
    public class CoachServiceTests
    {
        private readonly ICoachService _coachService;

        private readonly Mock<ICoachRepository> _coachRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly CoachEntity _fakeCoachEntity = new CoachEntity()
        {
            Id = 1,
        };

        private readonly CoachDTO _fakeCoachDTO = new CoachDTO()
        {
            Id = 1,
        };

        public CoachServiceTests()
        {
            _coachRepository = new Mock<ICoachRepository>();
            _mapper = new Mock<IMapper>();

            _coachService = new CoachService(_coachRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task ListAsync_WhenUnexpectedError_ThrowsException()
        {
            // Arrange
            var unexpectedEx = new Exception();
            _coachRepository.Setup(s => s.ListAsync()).ThrowsAsync(unexpectedEx);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await _coachService.ListAsync());
        }

        [Fact]
        public async Task ListAsync_WithoutConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var sqlEx = MakeSqlException();
            _coachRepository.Setup(s => s.ListAsync()).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _coachService.ListAsync());
        }

        [Fact]
        public async Task ListAsync_WhenCalled_ReturnsListCoachDTO()
        {
            // Arrange
            var fakeCoachEntityList = new List<CoachEntity>() { _fakeCoachEntity };
            var fakeCoachDTOList = new List<CoachDTO>() { _fakeCoachDTO };
            _coachRepository.Setup(s => s.ListAsync()).ReturnsAsync(fakeCoachEntityList);

            _mapper.Setup(s => s.Map<IList<CoachDTO>>(
                It.Is<IList<CoachEntity>>(i => i.Equals(fakeCoachEntityList)))).Returns(fakeCoachDTOList);

            // Act
            var result = await _coachService.ListAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveSameCount(fakeCoachEntityList);
            result.First().Should().NotBeNull();
            result.First().Should().BeOfType<CoachDTO>();
            result.First().Should().Be(_fakeCoachDTO);
        }

        [Fact]
        public async Task GetAsyncId_WhenUnexpectedError_ThrowsException()
        {
            // Arrange
            var unexpectEx = new Exception();
            _coachRepository.Setup(s => s.GetAsync(_fakeCoachEntity.Id)).ThrowsAsync(unexpectEx);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await _coachService.GetAsync(_fakeCoachDTO.Id));
        }

        [Fact]
        public async Task GetAsyncId_WithoutConnectionToDb_ThrowsSqlException()
        {
            // Arrange
            var sqlEx = MakeSqlException();
            _coachRepository.Setup(s => s.GetAsync(_fakeCoachEntity.Id)).ThrowsAsync(sqlEx);

            // Act and Assert
            await Assert.ThrowsAsync<SqlException>(async () => await _coachService.GetAsync(_fakeCoachDTO.Id));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetAsyncId_PassingInvalidId_ThrowsArgumentException(int id)
        {
            // Arrange, Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _coachService.GetAsync(id));
        }

        [Theory]
        [InlineData(10000)]
        [InlineData(1111111)]
        public async Task GetAsyncId_PassingNonExistentId_ThrowsNotFoundException(int id)
        {
            // Arrange
            CoachEntity nullUserEntity = null!;
            _coachRepository.Setup(s => s.GetAsync(id)).ReturnsAsync(nullUserEntity);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await _coachService.GetAsync(id));
        }

        [Fact]
        public async Task GetAsyncId_PassingValidId_ReturnsUserDTO()
        {
            // Arrange
            _coachRepository.Setup(s => s.GetAsync(_fakeCoachEntity.Id)).ReturnsAsync(_fakeCoachEntity);

            _mapper.Setup(s => s.Map<CoachDTO>(
                It.Is<CoachEntity>(i => i.Equals(_fakeCoachEntity)))).Returns(_fakeCoachDTO);

            // Act
            var result = await _coachService.GetAsync(_fakeCoachDTO.Id);

            // Assert
            result.Should().Be(_fakeCoachDTO);
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

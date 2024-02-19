namespace QuantumSport.API.Services
{
    public class CoachService : ICoachService
    {
        private readonly ICoachRepository _coachRepository;
        private readonly IMapper _mapper;

        public CoachService(ICoachRepository coachRepository, IMapper mapper)
        {
            _coachRepository = coachRepository;
            _mapper = mapper;
        }

        public async Task<CoachDTO> GetAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Invalid coach id");
            }

            var data = await _coachRepository.GetAsync(id);

            if (data == null)
            {
                throw new NotFoundException($"Coach with id = {id} does not exist");
            }

            return _mapper.Map<CoachDTO>(data);
        }

        public async Task<IList<CoachDTO>> ListAsync()
        {
            var data = await _coachRepository.ListAsync();
            return _mapper.Map<IList<CoachDTO>>(data);
        }
    }
}

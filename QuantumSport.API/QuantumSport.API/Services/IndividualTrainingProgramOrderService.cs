namespace QuantumSport.API.Services
{
    public class IndividualTrainingProgramOrderService : IIndividualTrainingProgramOrderService
    {
        private readonly IIndividualTrainingProgramOrderRepository _individualTrainingProgramOrderRepository;
        private readonly IUserService _userService;
        private readonly ICoachService _coachService;

        public IndividualTrainingProgramOrderService(
            IIndividualTrainingProgramOrderRepository individualTrainingProgramOrderRepository,
            IUserService userService,
            ICoachService coachService)
        {
            _individualTrainingProgramOrderRepository = individualTrainingProgramOrderRepository;
            _userService = userService;
            _coachService = coachService;
        }

        public async Task<int> AddAsync(IndividualTrainingProgramOrderRequestDTO trainingProgramOrderDTO)
        {
            UserDTO user = new UserDTO()
            {
                Name = trainingProgramOrderDTO.UserName,
                Phone = trainingProgramOrderDTO.UserPhone
            };

            await _coachService.GetAsync(trainingProgramOrderDTO.CoachId);

            try
            {
                user = await _userService.GetAsync(user.Phone);
            }
            catch (NotFoundException)
            {
                user.Id = await _userService.AddAsync(user);
            }

            IndividualTrainingProgramOrderEntity trainingProgramOrderEntity = new IndividualTrainingProgramOrderEntity()
            {
                UserId = user.Id,
                CoachId = trainingProgramOrderDTO.CoachId,
            };

            return await _individualTrainingProgramOrderRepository.AddAsync(trainingProgramOrderEntity);
        }
    }
}

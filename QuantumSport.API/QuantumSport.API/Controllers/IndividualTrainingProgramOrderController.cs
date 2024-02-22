namespace QuantumSport.API.Controllers
{
    [ApiController]
    public class IndividualTrainingProgramOrderController : ControllerBase
    {
        private readonly IIndividualTrainingProgramOrderService _individualTrainingProgramOrderService;
        private readonly IMapper _mapper;
        private readonly ILogger<IndividualTrainingProgramOrderController> _logger;

        public IndividualTrainingProgramOrderController(
            IIndividualTrainingProgramOrderService individualTrainingProgramOrderService,
            IMapper mapper,
            ILogger<IndividualTrainingProgramOrderController> logger)
        {
            _individualTrainingProgramOrderService = individualTrainingProgramOrderService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Route("/order/training-program")]
        public async Task<IActionResult> AddAsync(IndividualTrainingProgramOrderRequest trainingProgramOrderRequest)
        {
            try
            {
                var result = await _individualTrainingProgramOrderService
                    .AddAsync(_mapper.Map<IndividualTrainingProgramOrderRequestDTO>(trainingProgramOrderRequest));
                _logger.LogInformation($"Individual training program order with id = {result} was created");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }
    }
}

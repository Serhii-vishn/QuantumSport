namespace QuantumSport.API.Controllers
{
    [ApiController]
    public class CoachController : ControllerBase
    {
        private readonly ICoachService _coachService;
        private readonly IMapper _mapper;
        private readonly ILogger<CoachController> _logger;

        public CoachController(ICoachService coachService, IMapper mapper, ILogger<CoachController> logger)
        {
            _coachService = coachService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("/coach/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var result = await _coachService.GetAsync(id);
                return Ok(_mapper.Map<CoachModelResponse>(result));
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/coaches/short")]
        public async Task<IActionResult> ListShortAsync()
        {
            try
            {
                var result = await _coachService.ListAsync();
                return Ok(_mapper.Map<IList<CoachShortResponse>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/coaches/navigation")]
        public async Task<IActionResult> ListNavigationAsync()
        {
            try
            {
                var result = await _coachService.ListAsync();
                return Ok(_mapper.Map<IList<CoachNavigationResponse>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}

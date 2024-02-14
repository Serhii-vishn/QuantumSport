namespace QuantumSport.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("/users")]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.ListAsync();
                _logger.LogInformation($"Users (count = {result.Count}) were received");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/user/{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            try
            {
                var result = await _userService.GetAsync(id);
                _logger.LogInformation($"User with id = {result.Id} was received");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/user")]
        public async Task<ActionResult> GetUser(string phone)
        {
            try
            {
                var result = await _userService.GetAsync(phone);
                _logger.LogInformation($"User with id = {result.Id} was received");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("/user")]
        public async Task<ActionResult> AddUser(UserDTO user)
        {
            try
            {
                var result = await _userService.AddAsync(user);
                _logger.LogInformation($"User with id = {result} was added");
                return Ok(result);
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

        [HttpPut]
        [Route("/user")]
        public async Task<ActionResult> UpdateUser(UserDTO user)
        {
            try
            {
                var result = await _userService.UpdateAsync(user);
                _logger.LogInformation($"User with id = {result} was updated");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
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

        [HttpDelete]
        [Route("/user/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                _logger.LogInformation($"User with id = {result} was deleted");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }
    }
}

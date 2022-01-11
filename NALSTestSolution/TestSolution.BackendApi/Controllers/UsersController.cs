using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestSolution.Application.Model.Dtos;
using TestSolution.Application.System.User;
using TestSolution.Application.Utilities;

namespace TestSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserFormatter _userService;

        public UsersController(IUserFormatter userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                return BadRequest(Messages.ERRMSG4);
            //
            var result = await _userService.Authenticate(request);
            if (result.Status != ResponseStatus.OK)
            {
                return BadRequest(result.Message);
            }

            // OK
            return Ok(result.Message);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!request.Password.Equals(request.ConfirmPassword))
                return BadRequest(Messages.ERRMSG5);

            var result = await _userService.Register(request);
            if (result.Status != ResponseStatus.OK)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestSolution.Application.Model.Dtos;
using TestSolution.Application.System.User;
using TestSolution.Application.Utilities;
using TestSolution.Application.VerifyData;

namespace TestSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserFormatter _userFormatter;

        public UsersController(IUserFormatter userService)
        {
            _userFormatter = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            //
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //
            // Check valid data
            var verifyMsg = Verify.DoVerify(request);
            if (!string.IsNullOrEmpty(verifyMsg))
            {
                return BadRequest(verifyMsg);
            }

            // call service
            var result = await _userFormatter.Authenticate(request);
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

            // Check valid data
            var verifyMsg = Verify.DoVerify(request);
            if (!string.IsNullOrEmpty(verifyMsg))
            {
                return BadRequest(verifyMsg);
            }

            /// call service
            var result = await _userFormatter.Register(request);
            if (result.Status != ResponseStatus.OK)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TestSolution.Application.Model.Dtos;
using TestSolution.Application.Utilities;
using TestSolution.Data.Constants;
using TestSolution.WebApp.ApiIntegration;
using TestSolution.WebApp.UtilityHelpers;

namespace TestSolution.WebApp.Controllers
{
    public class UserloginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public UserloginController(IUserApiClient userApiClient,
          IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userApiClient.Authenticate(request);
            if (result.Status != ResponseStatus.OK)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }

            var userPrincipal = DataHelpers.ValidateToken(result.Message, _configuration);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };

            HttpContext.Session.SetString(SystemConstants.Token, result.Message);
            HttpContext.Session.SetString("FullName", userPrincipal.FindFirstValue(ClaimTypes.GivenName).ToString());

            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
                        
            return RedirectToAction(ActionName.Index, ControllerName.Home);
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(registerRequest);
            }

            var result = await _userApiClient.RegisterUser(registerRequest);
            if (result.Status != ResponseStatus.OK)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var loginResult = await _userApiClient.Authenticate(new LoginRequest()
            {
                UserName = registerRequest.UserName,
                Password = registerRequest.Password,
                RememberMe = true
            });

            var userPrincipal = DataHelpers.ValidateToken(loginResult.Message, _configuration);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };

            HttpContext.Session.SetString(SystemConstants.Token, loginResult.Message);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return RedirectToAction(ActionName.Login, ControllerName.Userlogin);
        }
        #endregion

        #region Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove(SystemConstants.Token);
            return RedirectToAction(ActionName.Login, ControllerName.Userlogin);
        }
        #endregion
    }
}

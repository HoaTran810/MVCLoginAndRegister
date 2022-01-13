using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestSolution.Application.Model.Dtos;
using TestSolution.Application.Utilities;
using TestSolution.Data.DataQuery.User;
using TestSolution.Data.Entities;

namespace TestSolution.Application.System.User
{
    public class UserFormatter : IUserFormatter
    {
        private readonly IUserQuery _userQuery;
        private readonly IConfiguration _config;

        public UserFormatter(IUserQuery userQuery, IConfiguration config)
        {
            _userQuery = userQuery;
            _config = config;
        }

        /// <summary>
        /// Check login
        /// </summary>
        /// <param name="request">infor for login</param>
        /// <returns></returns>
        public async Task<ResponseResult> Authenticate(LoginRequest request)
        {
            // Invalid data
            var errMsg = VerifyData.Verify.DoVerify(request);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return new ResponseResult
                {
                    Status = ResponseStatus.FAILED,
                    Message = errMsg
                };
            }

            // Find existed user
            var user = await _userQuery.FindByNameAsync(request.UserName);
            if (user == null)
                return new ResponseResult
                {
                    Status = ResponseStatus.FAILED,
                    Message = Messages.ERRMSG6
                };

            // Check user and password
            var result = await _userQuery.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
                return new ResponseResult
                {
                    Status = ResponseStatus.FAILED,
                    Message = Messages.ERRMSG7
                };

            // Roles
            var roles = await _userQuery.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.GivenName,user.FullName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Get token string
            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            // Success
            return new ResponseResult
            {
                Status = ResponseStatus.OK,
                Message = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        /// <summary>
        /// Register account login
        /// </summary>
        /// <param name="request">Data query</param>
        /// <returns></returns>
        public async Task<ResponseResult> Register(RegisterRequest request)
        {
            // Invalid data
            var errMsg = VerifyData.Verify.DoVerify(request);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return new ResponseResult
                {
                    Status = ResponseStatus.FAILED,
                    Message = errMsg
                };
            }

            // Find existed user
            var user = await _userQuery.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ResponseResult
                {
                    Status = ResponseStatus.FAILED,
                    Message = Messages.ERRMSG1
                };
            }

            // Object want to create
            user = new AppUser()
            {
                BirthDate = request.BirthDate,
                Email = request.Email,
                FullName = request.FullName,
                AccountType = request.AccountType,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };

            // Password verification is not strong enough
            var checkPwdMgs = await _userQuery.PasswordValidateAsync(request.Password);
            if (checkPwdMgs.Count > 0)
            {
                return new ResponseResult
                {
                    Status = ResponseStatus.FAILED,
                    Message = Messages.ERRMSG14
                };
            }

            // Process create
            var result = await _userQuery.CreateAsync(user, request.Password);
            return result.Succeeded ?
            new ResponseResult
            {
                Status = ResponseStatus.OK,
                Message = Messages.INFOMSG1
            }
            : new ResponseResult
            {
                Status = ResponseStatus.OK,
                Message = result.Errors.ToString()
            };
        }
    }
}

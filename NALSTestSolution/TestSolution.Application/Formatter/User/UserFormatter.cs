﻿using Microsoft.AspNetCore.Identity;
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

        public async Task<ResponseResult> Authenticate(LoginRequest request)
        {
            var user = await _userQuery.FindByNameAsync(request.UserName);
            if (user == null)
                return new ResponseResult
                {
                    Status = ResponseStatus.FAILED,
                    Message = Messages.ERRMSG6
                };

            var result = await _userQuery.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
                return new ResponseResult
                {
                    Status = ResponseStatus.FAILED,
                    Message = Messages.ERRMSG7
                };


            var roles = await _userQuery.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new ResponseResult
            {
                Status = ResponseStatus.OK,
                Message = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public async Task<ResponseResult> Register(RegisterRequest request)
        {
            var user = await _userQuery.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ResponseResult
                {
                    Status = ResponseStatus.FAILED,
                    Message = Messages.ERRMSG1
                };
            }

            user = new AppUser()
            {
                BirthDate = request.BirthDate,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };
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

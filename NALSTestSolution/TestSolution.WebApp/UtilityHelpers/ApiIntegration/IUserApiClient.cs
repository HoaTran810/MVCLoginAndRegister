using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSolution.Application.Model.Dtos;

namespace TestSolution.WebApp.ApiIntegration
{
    public interface IUserApiClient
    {
        Task<ResponseResult> Authenticate(LoginRequest request);

        Task<ResponseResult> RegisterUser(RegisterRequest registerRequest);
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestSolution.Application.Model.Dtos;
using TestSolution.Application.Utilities;

namespace TestSolution.WebApp.ApiIntegration
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseResult> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/users/Authenticate", httpContent);
            if (response.IsSuccessStatusCode)
            {
                var dataresponse = await response.Content.ReadAsStringAsync();
                return new ResponseResult
                {
                    Status = ResponseStatus.OK,
                    Message = dataresponse
                };
            }
                        
            return new ResponseResult
            {
                Status = ResponseStatus.FAILED,
                Message = await response.Content.ReadAsStringAsync()
            };
        }

        public async Task<ResponseResult> RegisterUser(RegisterRequest registerRequest)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(registerRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/users/Register", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return new ResponseResult
                {
                    Status = ResponseStatus.OK,
                    Message = result
                };

            return new ResponseResult
            {
                Status = ResponseStatus.FAILED,
                Message = result
            };
        }
    }
}

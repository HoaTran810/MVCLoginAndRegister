using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSolution.Application.Model.Dtos;

namespace TestSolution.Application.System.User
{
    public interface IUserService
    {
        public Task<string> Authenticate(LoginRequest request);

        public Task<bool> Register(RegisterRequest request);
    }
}

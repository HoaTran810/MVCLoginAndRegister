using System.Threading.Tasks;
using TestSolution.Application.Model.Dtos;

namespace TestSolution.Application.System.User
{
    public interface IUserFormatter
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request">params</param>
        /// <returns></returns>
        public Task<ResponseResult> Authenticate(LoginRequest request);

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="request">params</param>
        /// <returns></returns>
        public Task<ResponseResult> Register(RegisterRequest request);
    }
}

using TestSolution.Application.Model.Dtos;
using TestSolution.Application.Utilities;

namespace TestSolution.Application.VerifyData
{
    public static class Verify
    {
        /// <summary>
        /// Check data of object
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string DoVerify(object request)
        {
            // object is null
            if (request == null)
                return Messages.ERRMSG0;

            // LoginRequest
            if (request.GetType() == typeof(LoginRequest))
            {
                return Verify_LoginRequest((LoginRequest)request);
            }

            // RegisterRequest
            if (request.GetType() == typeof(RegisterRequest))
            {
                return Verify_RegisterRequest((RegisterRequest)request);
            }

            // Type not support
            return null;
        }

        /// <summary>
        /// Check data of RegisterRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string Verify_RegisterRequest(RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName))
                return Messages.ERRMSG8;//Tên đăng nhập không có.

            if (string.IsNullOrEmpty(request.Password))
                return Messages.ERRMSG9;//Mật khẩu không có.

            if (!request.Password.Equals(request.ConfirmPassword))
                return Messages.ERRMSG5;//Mật khẩu xác nhận không đúng.

            if (string.IsNullOrEmpty(request.Email))
                return Messages.ERRMSG10; //Email không có.

            if (string.IsNullOrEmpty(request.FullName))
                return Messages.ERRMSG11;// Họ tên không có

            if (string.IsNullOrEmpty(request.AccountType))
                return Messages.ERRMSG12;// AccountType không có

            if (string.IsNullOrEmpty(request.PhoneNumber))
                return Messages.ERRMSG13;// Số điện thoại không có

            return string.Empty;
        }

        /// <summary>
        /// Check data of LoginRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string Verify_LoginRequest(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName))
                return Messages.ERRMSG8;

            if (string.IsNullOrEmpty(request.Password))
                return Messages.ERRMSG9;

            return string.Empty;
        }
    }
}

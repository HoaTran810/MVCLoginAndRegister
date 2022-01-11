using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TestSolution.Application.Model.Dtos;
using TestSolution.Application.System.User;
using TestSolution.Application.Utilities;
using TestSolution.Data.DataQuery.User;
using TestSolution.Data.Entities;

namespace TestSolution.UnitTest
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class AccountLoginTest
    {
        private IConfiguration _config;
        private IUserQuery _userQuery;
        private IUserFormatter _userFormatter;

        [SetUp]
        public void Setup()
        {
            var userManagerMock = new Mock<UserManager<AppUser>>(
           /* IUserStore<TUser> store */Mock.Of<IUserStore<AppUser>>(),
           /* IOptions<IdentityOptions> optionsAccessor */null,
           /* IPasswordHasher<TUser> passwordHasher */null,
           /* IEnumerable<IUserValidator<TUser>> userValidators */null,
           /* IEnumerable<IPasswordValidator<TUser>> passwordValidators */null,
           /* ILookupNormalizer keyNormalizer */null,
           /* IdentityErrorDescriber errors */null,
           /* IServiceProvider services */null,
           /* ILogger<UserManager<TUser>> logger */null);


            var signInManagerMock = new Mock<SignInManager<AppUser>>(
                userManagerMock.Object,
                /* IHttpContextAccessor contextAccessor */Mock.Of<IHttpContextAccessor>(),
                /* IUserClaimsPrincipalFactory<TUser> claimsFactory */Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                /* IOptions<IdentityOptions> optionsAccessor */null,
                /* ILogger<SignInManager<TUser>> logger */null,
                /* IAuthenticationSchemeProvider schemes */null,
                /* IUserConfirmation<TUser> confirmation */null);

            // Arrange
            var inMemorySettings = new Dictionary<string, string> {
                {"Tokens:Key", "ABCDEF0123401234"},
                {"Tokens:Issuer", "https://nals.vn"}
            };
            _config = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemorySettings)
                                .Build();

            _userQuery = new UserQuery(userManagerMock.Object, signInManagerMock.Object);
            _userFormatter = new UserFormatter(_userQuery, _config);

        }
        
        #region Empty Parameter

        [TestCase]
        public void Test_Login_EmptyUsername()
        {
            /*If Username is empty then return failed.*/

            // Result
            var result = _userFormatter.Authenticate(new LoginRequest()
            {
                UserName = "",
                Password = "Abc!23",                
                RememberMe = true
            });

            // Expected
            var expected = new ResponseResult()
            {
                Status = ResponseStatus.FAILED,
                Message = Messages.ERRMSG8
            };

            Assert.AreEqual(result.Result.Status, expected.Status);
            Assert.AreEqual(result.Result.Message, expected.Message);
        }

        [TestCase]
        public void Test_Login_EmptyPassword()
        {
            /*If Password is empty then return failed.*/

            // Result
            var result = _userFormatter.Authenticate(new LoginRequest()
            {
                Password = "",
                UserName = "admin",
                RememberMe = true
            });

            // Expected
            var expected = new ResponseResult()
            {
                Status = ResponseStatus.FAILED,
                Message = Messages.ERRMSG9
            };

            Assert.AreEqual(result.Result.Status, expected.Status);
            Assert.AreEqual(result.Result.Message, expected.Message);
        }

        [TestCase]
        public void Test_Login_EmptyUsernameAndPassword()
        {
            /*If Password and Username is empty then return failed.*/

            // Result
            var result = _userFormatter.Authenticate(new LoginRequest()
            {
                Password = "",
                UserName = "",
                RememberMe = true
            });

            // Expected
            var expected = new ResponseResult()
            {
                Status = ResponseStatus.FAILED,
                Message = Messages.ERRMSG8
            };

            Assert.AreEqual(result.Result.Status, expected.Status);
            Assert.AreEqual(result.Result.Message, expected.Message);
        } 
        #endregion
    }
}

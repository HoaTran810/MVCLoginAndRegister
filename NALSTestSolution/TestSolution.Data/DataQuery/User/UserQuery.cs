using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestSolution.Data.Entities;

namespace TestSolution.Data.DataQuery.User
{
    public class UserQuery: IUserQuery
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;                

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public UserQuery(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;            
        }

        /// <summary>
        /// FindByNameAsync
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Task<AppUser> FindByNameAsync(string userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        /// <summary>
        /// GetRolesAsync
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<string>> GetRolesAsync(AppUser user)
        {
            return _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// PasswordSignInAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <param name="lockoutOnFailure"></param>
        /// <returns></returns>
        public Task<SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        /// <summary>
        /// CreateAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return _userManager.CreateAsync(user, password);         
        }

        /// <summary>
        /// CheckPasswordAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Check password meet the conditions
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<List<string>> PasswordValidateAsync(string password)
        {
            var passwordErrors = new List<string>();

            // All of conditions
            var validators = _userManager.PasswordValidators;

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(_userManager, null, password);

                if (!result.Succeeded)
                {
                    // Error
                    foreach (var error in result.Errors)
                    {
                        passwordErrors.Add(error.Description);
                    }
                }
            }

            return passwordErrors;
        }
    }
}

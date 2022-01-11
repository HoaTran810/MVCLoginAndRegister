using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSolution.Data.Entities;

namespace TestSolution.Data.DataQuery.User
{
    public interface IUserQuery
    {
        /// <summary>
        /// FindByNameAsync
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Task<AppUser> FindByNameAsync(string userName);

        /// <summary>
        /// GetRolesAsync
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<string>> GetRolesAsync(AppUser user);

        /// <summary>
        /// PasswordSignInAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <param name="lockoutOnFailure"></param>
        /// <returns></returns>
        public Task<SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure);

        /// <summary>
        /// CreateAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateAsync(AppUser user, string password);

        /// <summary>
        /// CheckPasswordAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<bool> CheckPasswordAsync(AppUser user, string password);

    }
}

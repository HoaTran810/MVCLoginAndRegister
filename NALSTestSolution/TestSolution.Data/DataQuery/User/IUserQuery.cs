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
        public Task<AppUser> FindByNameAsync(string userName);

        public Task<IList<string>> GetRolesAsync(AppUser user);

        public Task<SignInResult> PasswordSignInAsync(AppUser user, string password, bool isPersistent, bool lockoutOnFailure);

        public Task<IdentityResult> CreateAsync(AppUser user, string password);

    }
}

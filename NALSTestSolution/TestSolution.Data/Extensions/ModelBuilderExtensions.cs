using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSolution.Data.Entities;

namespace TestSolution.Data.Extentions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var roleId = new Guid ("ADE97465-7A86-4215-A19A-F753A88EE4A1");
;
            var adminId = new Guid("B81A303B-161C-4F45-B3BF-1903319A1CF7");

            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "dotest@gmail.com",
                NormalizedEmail = "dotest@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Abc@123"),
                SecurityStamp = string.Empty,
                FirstName = "Hoa",
                LastName = "Tran",
                BirthDate = new DateTime(2020, 01, 07)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });
        }
    }
}

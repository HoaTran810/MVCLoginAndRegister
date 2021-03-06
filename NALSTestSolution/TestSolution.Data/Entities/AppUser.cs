using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolution.Data.Entities
{
    public class AppUser: IdentityUser<Guid>
    {
        public string FullName { get; set; }

        public string AccountType { get; set; }

        public DateTime BirthDate { get; set; }
    }
}

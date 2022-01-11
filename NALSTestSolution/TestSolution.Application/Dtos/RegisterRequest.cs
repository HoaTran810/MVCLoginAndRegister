using System;
using System.ComponentModel.DataAnnotations;

namespace TestSolution.Application.Model.Dtos
{
    public class RegisterRequest
    {
        public string FullName { get; set; }

        public string AccountType { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

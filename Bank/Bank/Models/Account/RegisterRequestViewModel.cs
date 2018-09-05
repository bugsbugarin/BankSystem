using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models.Account
{
    public class RegisterRequestViewModel
    {
        [Required, MaxLength(256), DisplayName("Username")]
        public string LoginName { get; set; }

        public string AccountNumber { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Password did not match"), DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        public decimal Balance { get; set; }

        public DateTime  CreateDate { get; set; }
    }
}

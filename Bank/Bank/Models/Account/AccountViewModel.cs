using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models.Account
{
    public class AccountViewModel
    {
        [DisplayName("Login Name")]
        public string LoginName { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }

        [DisplayName("Create Date")]
        public DateTime CreateDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models.Account
{
    public class LoginResponseViewModel
    {
        public long Id { get; set; }

        public string LoginName { get; set; }

        public string AccountNumber { get; set; }
    }
}

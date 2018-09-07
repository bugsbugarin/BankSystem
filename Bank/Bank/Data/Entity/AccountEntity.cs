using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Data.Entity
{
    public class AccountEntity
    {
        public long Id { get; set; }

        public string LoginName { get; set; }

        public string AccountNumber { get; set; }

        public string Password { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Data.Entity
{
    public class TransactionLogEntity
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public int TransactionType { get; set; }

        public decimal Amount { get; set; }

        public long DestinationAccountId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string SourceAccountNumber { get; set; }

        public string DestinationAccountNumber { get; set; }

        public int TotalRows { get; set; }
    }
}

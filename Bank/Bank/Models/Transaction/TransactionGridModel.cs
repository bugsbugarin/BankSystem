using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Bank.Helper.Enums;

namespace Bank.Models.Transaction
{
    public class TransactionGridModelList
    {
       public List<TransactionGridModel> Transactions { get; set; }
        
        public int TotalRows { get; set; }
    }

    public class TransactionGridModel
    {
        public string TransactionId { get; set; }

        public string TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string ReceiverAccountNumber { get; set; }

        public DateTime TransactionDate { get; set; }

        public string SourceAccountNumber { get; set; }
    }
}

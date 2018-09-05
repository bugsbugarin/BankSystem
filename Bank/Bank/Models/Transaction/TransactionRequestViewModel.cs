using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Bank.Helper.Enums;

namespace Bank.Models.Transaction
{
    public class TransactionRequestViewModel
    {
        public long AccountId { get; set; }

        [Required]
        [DisplayName("Transaction Type")]
        public int TransactionType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [DisplayName("Destination Account Number")]
        public string ReceiverAccountNumber { get; set; }
    }
}

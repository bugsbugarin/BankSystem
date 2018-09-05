using Bank.Models.Account;
using Bank.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Domain.Interface
{
    public interface ITransactionManager
    {
        void Deposit(TransactionRequestViewModel request);

        void Withdraw(TransactionRequestViewModel request);

        void Transfer(TransactionRequestViewModel request);

        TransactionGridModelList SearchSentTransaction(long accountId, int pageSize, int pageNumber);

        TransactionGridModelList SearchReceivedTransaction(long accountId, int pageSize, int pageNumber);
    }
}

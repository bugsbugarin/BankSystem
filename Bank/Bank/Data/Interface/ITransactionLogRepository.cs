using Bank.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Data.Interface
{
    public interface ITransactionLogRepository
    {
        void Insert(TransactionLogEntity request);

        List<TransactionLogEntity> SearchSent(long accountId, int pageSize, int pageNumber);

        List<TransactionLogEntity> SearchReceived(long accountId, int pageSize, int pageNumber);
    }
}

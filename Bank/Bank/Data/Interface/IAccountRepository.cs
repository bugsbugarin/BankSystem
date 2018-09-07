using Bank.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Data.Interface
{
    public interface IAccountRepository
    {
        void Insert(AccountEntity request);

        void Update(AccountEntity request);

        AccountEntity GetById(long id);

        AccountEntity GetByLoginName(string username);

        AccountEntity GetByAccountNumber(string username);

        long GetNextAccountId(string loginName);
    }
}

using Bank.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Domain.Interface
{
    public interface IAccountManager
    {
        void Register(RegisterRequestViewModel request);

        LoginResponseViewModel Login(LoginRequestViewModel request);

        AccountViewModel GetById(long id);
    }
}

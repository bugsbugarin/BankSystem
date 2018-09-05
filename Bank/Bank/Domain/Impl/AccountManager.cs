using Bank.Data.Entity;
using Bank.Domain.Interface;
using Bank.Models.Account;
using System;
using Bank.Helper;
using Bank.Data.Interface;
using Microsoft.Extensions.Configuration;
using Serilog;
using Bank.Exception;

namespace Bank.Domain.Impl
{
    public class AccountManager : IAccountManager
    {
        IAccountRepository _accountRepository;
        private static IConfiguration _config;

        public AccountManager(IAccountRepository accountRepository, IConfiguration config)
        {
            _accountRepository = accountRepository;
            _config = config;
        } 

        public void Register(RegisterRequestViewModel request)
        {
            Log.Information("Start - Registration request for user: {0}", request.LoginName);
            var account = _accountRepository.GetByLoginName(request.LoginName);

            if (account != null && account.Id != 0)
            {
                CommonHelper.ThrowAppException(string.Format("Login name {0} already exist!", request.LoginName));
            }

            _accountRepository.Insert(new AccountEntity()
                                        {
                                            //TODO Auto Generate
                                            AccountNumber = CommonHelper.GenerateAccountNumber(_accountRepository.GetNextAccountId(request.LoginName)),
                                            Balance = request.Balance,
                                            CreateDate = DateTime.Now,
                                            LoginName = request.LoginName,
                                            Password = CommonHelper.EncodePasswordToBase64(request.Password, _config.GetValue<string>("HashKey")),
                                        });

            Log.Information("End - Registration request for user: {0}", request.LoginName);
        }

        public LoginResponseViewModel Login(LoginRequestViewModel request)
        {
            Log.Information("Start - Login request for user: {0}", request.LoginName);

            LoginResponseViewModel ret = null;
            //1 - validation
            var account = _accountRepository.GetByLoginName(request.LoginName);

            //2 - Check if account exist
            if (account != null && account.Id != 0)
            {
                //3 - Check if password is correct

                if (!String.Equals(CommonHelper.EncodePasswordToBase64(request.Password, _config.GetValue<string>("HashKey")), account.Password))
                {
                    CommonHelper.ThrowAppException("Username or Password is incorrect!");
                }

                ret = new LoginResponseViewModel()
                {
                    LoginName = account.LoginName,
                    Id = account.Id,
                    AccountNumber = account.AccountNumber
                };
            }

            Log.Information("End - Login request for user: {0}", request.LoginName);

            return ret;
        }

        public AccountViewModel GetById(long id)
        {
            AccountViewModel ret = null;
            //1 - validation
            var account = _accountRepository.GetById(id);

            //2 - Check if account exist
            if (account != null && account.Id != 0)
            {

                ret = new AccountViewModel()
                {
                    LoginName = account.LoginName,
                    Balance = account.Balance,
                    AccountNumber = account.AccountNumber,
                    CreateDate = account.CreateDate
                };
            }

            return ret;
        }
    }
}

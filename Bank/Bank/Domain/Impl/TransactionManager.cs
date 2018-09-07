using Bank.Data.Entity;
using Bank.Data.Interface;
using Bank.Domain.Interface;
using Bank.Helper;
using Bank.Models.Transaction;
using Serilog;
using System;
using System.Collections.Generic;
using System.Transactions;
using static Bank.Helper.Enums;

namespace Bank.Domain.Impl
{
    public class TransactionManager : ITransactionManager
    {
        private ITransactionLogRepository _transactionRepository;
        private IAccountRepository _accountRepository;

        public TransactionManager(ITransactionLogRepository transactionLogRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionLogRepository;
            _accountRepository = accountRepository;
        }

        public void Deposit(TransactionRequestViewModel request)
        {
            Log.Information("Start - Deposit request to Account Id {0}", request.AccountId);
            using (TransactionScope scope = new TransactionScope())
            {
                var account = _accountRepository.GetById(request.AccountId);
                account.Balance += request.Amount;
                _accountRepository.Update(account);

                _transactionRepository.Insert(new TransactionLogEntity()
                {
                    Amount = request.Amount,
                    TransactionDate = DateTime.Now,
                    TransactionType = request.TransactionType,
                    AccountId = request.AccountId,
                });

                scope.Complete();
            }

            Log.Information("End - Deposit request to Account Id {0}", request.AccountId);
        }

        public void Transfer(TransactionRequestViewModel request)
        {
            Log.Information("Start - Transfer request from Account Id {0} to Account Number {1} ", request.AccountId, request.ReceiverAccountNumber);
            AccountEntity destinationAccount = new AccountEntity();

            if (!string.IsNullOrEmpty(request.ReceiverAccountNumber))
            {
                destinationAccount = _accountRepository.GetByAccountNumber(request.ReceiverAccountNumber);
            }

            if(destinationAccount == null || destinationAccount.Id == 0)
            {
                CommonHelper.ThrowAppException("Destination account number does not exist.");
            }

            if (destinationAccount.Id == request.AccountId)
            {
                CommonHelper.ThrowAppException("Invalid destination account.");
            }

            var sourceAccount = _accountRepository.GetById(request.AccountId);

            if (sourceAccount.Balance < request.Amount)
            {
                CommonHelper.ThrowAppException("Insufficient funds");
            }

            sourceAccount.Balance -= request.Amount;
            destinationAccount.Balance += request.Amount;

            using (TransactionScope scope = new TransactionScope())
            {

                _accountRepository.Update(sourceAccount);
                _accountRepository.Update(destinationAccount);
                _transactionRepository.Insert(new TransactionLogEntity()
                {
                    Amount = request.Amount,
                    TransactionDate = DateTime.Now,
                    TransactionType = request.TransactionType,
                    AccountId = request.AccountId,
                    DestinationAccountId = destinationAccount.Id
                });

                scope.Complete();
            }

            Log.Information("End - Transfer request from Account Id {0} to Account Number {1} ", request.AccountId, request.ReceiverAccountNumber);
        }

        public void Withdraw(TransactionRequestViewModel request)
        {
            Log.Information("Start - Withdraw request to Account Id {0}", request.AccountId);

            var accountEntity = _accountRepository.GetById(request.AccountId);

            if (accountEntity.Balance < request.Amount)
            {
                CommonHelper.ThrowAppException("Insufficient funds");
            }
            using (TransactionScope scope = new TransactionScope())
            {
                accountEntity.Balance -= request.Amount;
                _accountRepository.Update(accountEntity);

                _transactionRepository.Insert(new TransactionLogEntity()
                {
                    Amount = request.Amount,
                    TransactionDate = DateTime.Now,
                    TransactionType = request.TransactionType,
                    AccountId = request.AccountId,
                });

                scope.Complete();
            }

            Log.Information("End - Withdraw request to Account Id {0}", request.AccountId);
        }

        public TransactionGridModelList SearchSentTransaction(long accountId, int pageSize, int pageNumber)
        {
            TransactionGridModelList ret = new TransactionGridModelList();

            var transactions = _transactionRepository.SearchSent(accountId, pageSize, pageNumber);

            if (transactions != null)
            {
                ret.Transactions = new List<TransactionGridModel>();
                transactions.ForEach(x =>
                {
                    ret.TotalRows = x.TotalRows;
                    ret.Transactions.Add(new TransactionGridModel()
                    {
                        Amount = x.Amount,
                        ReceiverAccountNumber = x.DestinationAccountNumber,
                        SourceAccountNumber = x.SourceAccountNumber,
                        TransactionDate = x.TransactionDate,
                        TransactionId = x.Id.ToString(),
                        TransactionType = ((TransactionType)x.TransactionType).ToString(),

                    });
                });
            }

            return ret;
        }

        public TransactionGridModelList SearchReceivedTransaction(long accountId, int pageSize, int pageNumber)
        {
            TransactionGridModelList ret = new TransactionGridModelList();

            var transactions = _transactionRepository.SearchReceived(accountId, pageSize, pageNumber);

            if (transactions != null)
            {
                ret.Transactions = new List<TransactionGridModel>();
                transactions.ForEach(x =>
                {
                    ret.TotalRows = x.TotalRows;
                    ret.Transactions.Add(new TransactionGridModel()
                    {
                        Amount = x.Amount,
                        ReceiverAccountNumber = x.DestinationAccountNumber,
                        SourceAccountNumber = x.SourceAccountNumber,
                        TransactionDate = x.TransactionDate,
                        TransactionId = x.Id.ToString(),
                        TransactionType = ((TransactionType)x.TransactionType).ToString(),

                    });
                });
            }

            return ret;
        }
    }
}

using Bank.Data.Entity;
using Bank.Data.Interface;
using Bank.Exception;
using Bank.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Data.Impl
{
    public class AccountRepository : IAccountRepository
    {
        public void Insert(AccountEntity request)
        {
            var parameter = new[]
            {
                new SqlParameter("@Balance", request.Balance),
                new SqlParameter("@LoginName", request.LoginName),
                new SqlParameter("@AccountNumber", request.AccountNumber),
                new SqlParameter("@CreateDate", request.CreateDate),
                new SqlParameter("@Password", request.Password),
            };
            using (var conn = new SqlConnection(SQLHelper.GetConnectionString()))
            {
                SQLHelper.ExecuteNonQuery(
                conn,
                CommandType.Text,
                @"INSERT INTO dbo.Account
                    (Balance, LoginName, AccountNumber, CreateDate, Password, LastUpdate)
                    SELECT @Balance, @LoginName, @AccountNumber, @CreateDate, @Password, GETUTCDATE()",
                parameter);
            }
        }

        public void Update(AccountEntity entity)
        {
            var parameter = new[]
            {
                new SqlParameter("@Id", entity.Id),
                new SqlParameter("@Balance", entity.Balance),
                new SqlParameter("@LastUpdate", entity.LastUpdate),
            };

            using (var conn = new SqlConnection(SQLHelper.GetConnectionString()))
            {
                int ret = SQLHelper.ExecuteNonQuery(
                                    conn,
                                    CommandType.Text,
                                    @"UPDATE 
                                        dbo.Account
                                        SET
                                        Balance = @Balance,
                                        LastUpdate = GETUTCDATE()
                                        WHERE
                                        Id = @Id AND
                                        LastUpdate = @LastUpdate",
                                    parameter);

                if(ret != 1)
                {
                    throw new AppException("Update fail. Record is not updated.");
                }
            }
        }

        public AccountEntity GetById(long id)
        {
            var ret = new AccountEntity();
            var parameter = new[]
            {
                new SqlParameter("@Id", id)
            };
            using (var conn = new SqlConnection(SQLHelper.GetConnectionString()))
            {
                SqlDataReader rdr = SQLHelper.ExecuteReader(
                                   conn,
                                   CommandType.Text,
                                   @" SELECT 
                                        Id,
                                        LoginName,
                                        AccountNumber,
                                        Balance,
                                        CreateDate,
                                        LastUpdate
                                      FROM
                                        dbo.Account
                                      WHERE
                                        Id = @Id",
                                   parameter);

                ret = AccountEntityMapper.ToEntity(rdr);
            }

            return ret;
        }

        public AccountEntity GetByLoginName(string loginName)
        {
            var ret = new AccountEntity();
            var parameter = new[]
            {
                new SqlParameter("@LoginName", loginName)
            };
            using (var conn = new SqlConnection(SQLHelper.GetConnectionString()))
            {
                SqlDataReader rdr = SQLHelper.ExecuteReader(
                                   conn,
                                   CommandType.Text,
                                   @" SELECT 
                                        Id,
                                        LoginName,
                                        AccountNumber,
                                        Balance,
                                        CreateDate,
                                        Password
                                      FROM
                                        dbo.Account
                                      WHERE
                                        LoginName = @LoginName",
                                   parameter);

                ret = AccountEntityMapper.ToEntityWithPassword(rdr);
            }

            return ret;
        }

        public AccountEntity GetByAccountNumber(string accountNumber)
        {
            var ret = new AccountEntity();
            var parameter = new[]
            {
                new SqlParameter("@AccountNumber", accountNumber)
            };
            using (var conn = new SqlConnection(SQLHelper.GetConnectionString()))
            {
                SqlDataReader rdr = SQLHelper.ExecuteReader(
                                   conn,
                                   CommandType.Text,
                                   @" SELECT 
                                        Id,
                                        LoginName,
                                        AccountNumber,
                                        Balance,
                                        CreateDate,
                                        LastUpdate
                                      FROM
                                        dbo.Account
                                      WHERE
                                        AccountNumber = @AccountNumber",
                                   parameter);

                ret = AccountEntityMapper.ToEntity(rdr);
            }

            return ret;
        }

        public long GetNextAccountId(string loginName)
        {
            var parameter = new[]
            {
                new SqlParameter("@LoginName", loginName),
            };
            using (var conn = new SqlConnection(SQLHelper.GetConnectionString()))
            {
              SqlDataReader rdr =  SQLHelper.ExecuteReader(
                                        conn,
                                        CommandType.Text,
                                        @"INSERT INTO
                                            dbo.AccountNumber
                                            (LoginName)
                                          SELECT @LoginName;
                                          SELECT @@IDENTITY AS 'AccountNumber'",
                                        parameter);

                return AccountEntityMapper.ToAccountNumberEntity(rdr);
            }
        }
    }
}

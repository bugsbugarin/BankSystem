using Bank.Data.Entity;
using Bank.Data.Interface;
using Bank.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Bank.Data.Impl
{
    public class TransactionLogRepository : ITransactionLogRepository
    {
        public void Insert(TransactionLogEntity request)
        {
            var parameter = new[]
            {
                new SqlParameter("@AccountId", request.AccountId),
                new SqlParameter("@TransactionType", request.TransactionType),
                new SqlParameter("@TransactionDate", request.TransactionDate),
                new SqlParameter("@Amount", request.Amount),
                new SqlParameter("@DestinationAccountId", request.DestinationAccountId),
            };
            using (var conn = new SqlConnection(SQLHelper.GetConnectionString()))
            {
                SQLHelper.ExecuteNonQuery(
                conn,
                CommandType.Text,
                @"INSERT INTO dbo.TransactionLog
                    (AccountId, TransactionType, TransactionDate, Amount, DestinationAccountId)
                    SELECT @AccountId, @TransactionType, @TransactionDate, @Amount, @DestinationAccountId",
                parameter);
            }
        }

        public List<TransactionLogEntity> SearchSent(long accountId, int pageSize, int pageNumber)
        {
            var ret = new List<TransactionLogEntity>();
            var parameter = new[]
            {
                new SqlParameter("@AccountId", accountId),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@PageNumber", pageNumber),
            };
            using (var conn = new SqlConnection(SQLHelper.GetConnectionString()))
            {
                SqlDataReader rdr = SQLHelper.ExecuteReader(
                                   conn,
                                   CommandType.Text,
                                   @" SELECT 
                                        tl.Id,
                                        tl.TransactionType,
                                        tl.TransactionDate,
                                        tl.AccountId,
                                        tl.Amount,
                                        tl.DestinationAccountId,
                                        src.AccountNumber AS 'SourceAccountNumber',
                                        des.AccountNumber AS 'DestinationAccountNumber',
                                        TotalRows = COUNT(*) OVER()    
                                      FROM
                                        dbo.TransactionLog tl
                                      LEFT JOIN
                                        dbo.Account src on tl.AccountId = src.Id
                                     LEFT JOIN
                                        dbo.Account des on tl.DestinationAccountId = des.Id
                                      WHERE
                                        AccountId = @AccountId
                                      ORDER BY
                                            tl.Id DESC
                                      OFFSET @PageSize * (@PageNumber - 1) ROWS
                                      FETCH NEXT @PageSize ROWS ONLY",
                                   parameter);

                ret = TransactionLogEntityMapper.ToListEntity(rdr);
            }

            return ret;
        }

        public List<TransactionLogEntity> SearchReceived(long accountId, int pageSize, int pageNumber)
        {
            var ret = new List<TransactionLogEntity>();
            var parameter = new[]
            {
                new SqlParameter("@AccountId", accountId),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@PageNumber", pageNumber),
            };
            using (var conn = new SqlConnection(SQLHelper.GetConnectionString()))
            {
                SqlDataReader rdr = SQLHelper.ExecuteReader(
                                   conn,
                                   CommandType.Text,
                                   @" SELECT 
                                        tl.Id,
                                        tl.TransactionType,
                                        tl.TransactionDate,
                                        tl.AccountId,
                                        tl.Amount,
                                        tl.DestinationAccountId,
                                        src.AccountNumber AS 'SourceAccountNumber',
                                        des.AccountNumber AS 'DestinationAccountNumber',
                                        TotalRows = COUNT(*) OVER()    
                                      FROM
                                        dbo.TransactionLog tl
                                      LEFT JOIN
                                        dbo.Account src on tl.DestinationAccountId = src.Id
                                     LEFT JOIN
                                        dbo.Account des on tl.DestinationAccountId = des.Id
                                      WHERE
                                        DestinationAccountId = @AccountId
                                      ORDER BY
                                            tl.Id DESC
                                      OFFSET @PageSize * (@PageNumber - 1) ROWS
                                      FETCH NEXT @PageSize ROWS ONLY",
                                   parameter);

                ret = TransactionLogEntityMapper.ToListEntity(rdr);
            }

            return ret;
        }
    }
}

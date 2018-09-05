using Bank.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Data.Impl
{
    public class TransactionLogEntityMapper
    {
        public static List<TransactionLogEntity> ToListEntity(SqlDataReader rdr)
        {
            List<TransactionLogEntity> ret = new List<TransactionLogEntity>();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    ret.Add(new TransactionLogEntity()
                    {
                        AccountId = long.Parse(rdr["AccountId"].ToString()),
                        Amount = int.Parse(rdr["Amount"].ToString()),
                        TransactionDate = DateTime.Parse(rdr["TransactionDate"].ToString()),
                        Id = long.Parse(rdr["Id"].ToString()),
                        TransactionType = int.Parse(rdr["TransactionType"].ToString()),
                        DestinationAccountId = long.Parse(rdr["DestinationAccountId"].ToString()),
                        DestinationAccountNumber = rdr["DestinationAccountNumber"].ToString(),
                        SourceAccountNumber = rdr["SourceAccountNumber"].ToString(),
                        TotalRows = int.Parse(rdr["TotalRows"].ToString()),
                    });
                }
            }
            return ret;
        }
    }
}

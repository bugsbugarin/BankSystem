using Bank.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Data.Impl
{
    public class AccountEntityMapper
    {
        public static AccountEntity ToEntity(SqlDataReader rdr)
        {
            AccountEntity ret = new AccountEntity();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    ret = new AccountEntity()
                    {
                        AccountNumber = rdr["AccountNumber"].ToString(),
                        Balance = int.Parse(rdr["Balance"].ToString()),
                        CreateDate = DateTime.Parse(rdr["CreateDate"].ToString()),
                        Id = long.Parse(rdr["Id"].ToString()),
                        LoginName = rdr["LoginName"].ToString(),
                        Version = (byte[])rdr["Version"]
                    };
                }
            }

            return ret;
        }
        public static AccountEntity ToEntityWithPassword(SqlDataReader rdr)
        {
            AccountEntity ret = new AccountEntity();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    ret = new AccountEntity()
                    {
                        AccountNumber = rdr["AccountNumber"].ToString(),
                        Balance = int.Parse(rdr["Balance"].ToString()),
                        CreateDate = DateTime.Parse(rdr["CreateDate"].ToString()),
                        Id = long.Parse(rdr["Id"].ToString()),
                        LoginName = rdr["LoginName"].ToString(),
                        Password = rdr["Password"].ToString(),
                    };
                }
            }
            return ret;
        }

        public static long ToAccountNumberEntity(SqlDataReader rdr)
        {
            long ret = 0;
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    ret = long.Parse(rdr["AccountNumber"].ToString());
                }
            }
            return ret;
        }
    }
}

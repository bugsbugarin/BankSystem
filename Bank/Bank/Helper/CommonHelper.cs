using Bank.Exception;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Helper
{
    public static class CommonHelper
    {
        public static string EncodePasswordToBase64(string password, string hashKey)
        {
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + hashKey));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            }
        }

        public static void ThrowAppException(string errorMessage)
        {
            Log.Error(errorMessage);
            throw new AppException(errorMessage);
        }

        public static string GenerateAccountNumber(long id)
        {
            StringBuilder sb = new StringBuilder();
            string str = id.ToString();
            if ((10 - str.Length) > 0)
            {
                sb.Append('0', (10 - str.Length));
            }
            sb.Append(str);
            return sb.ToString();
        }
    }
}

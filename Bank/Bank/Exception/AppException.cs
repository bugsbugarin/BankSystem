using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Exception
{
    public class AppException : System.Exception
    {
        public AppException()
        { }

        public AppException(string message)
            : base(message)
        { }

        public AppException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}

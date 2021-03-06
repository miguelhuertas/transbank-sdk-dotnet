using System;

namespace Transbank.Exceptions
{
    public class MallTransactionRefundException : TransbankException
    {
        public MallTransactionRefundException(string message) : base(-1, message) { }

        public MallTransactionRefundException(int code, string message) : base(code, message) { }

        public MallTransactionRefundException(int code, string message, Exception innerException)
            : base(code, message, innerException) { }
    }
}

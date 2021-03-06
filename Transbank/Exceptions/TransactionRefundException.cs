﻿using System;

namespace Transbank.Exceptions
{
    public class TransactionRefundException : TransbankException
    {
        public TransactionRefundException(string message) : base(-1, message) { }

        public TransactionRefundException(int code, string message) : base(code, message) { }

        public TransactionRefundException(int code, string message, Exception innerException)
            : base(code, message, innerException) { }
    }
}

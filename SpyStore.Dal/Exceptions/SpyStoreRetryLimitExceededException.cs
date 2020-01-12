using System;

namespace SpyStore.Dal.Exceptions
{
    public class SpyStoreRetryLimitException: SpyStoreException
    {
        public SpyStoreRetryLimitException() { }
        public SpyStoreRetryLimitException(string message) : base(message) { }
        public SpyStoreRetryLimitException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }

}
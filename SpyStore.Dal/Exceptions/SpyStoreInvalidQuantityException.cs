using System;

namespace SpyStore.Dal.Exceptions
{
    public class SpyStoreQuantityException: SpyStoreException
    {
        public SpyStoreQuantityException() { }
        public SpyStoreQuantityException(string message) : base(message) { }
        public SpyStoreQuantityException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }

}
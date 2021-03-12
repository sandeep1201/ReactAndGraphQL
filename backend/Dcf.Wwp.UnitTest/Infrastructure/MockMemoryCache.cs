using Microsoft.Extensions.Caching.Memory;

namespace Dcf.Wwp.UnitTest.Infrastructure
{
    public class MockMemoryCache : IMemoryCache
    {
        #region Properties

        #endregion

        #region Methods

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(object key, out object value)
        {
            throw new System.NotImplementedException();
        }

        public ICacheEntry CreateEntry(object key)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}

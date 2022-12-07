using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Cache
{
    public class AppealCache
    {
        private TvChannelContext db;
        private IMemoryCache cache;
        private int _rowsNumber;

        public AppealCache(TvChannelContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
        }

        public void Add(string cacheKey)
        {
            IEnumerable<Appeal> items = db.Appeals.Take(_rowsNumber);

            cache.Set(cacheKey, items, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }

        public IEnumerable<Appeal> Get(string cacheKey)
        {
            IEnumerable<Appeal> items = null;
            if (!cache.TryGetValue(cacheKey, out items))
            {
                items = db.Appeals.Take(_rowsNumber).ToList();
                if (items != null)
                {
                    cache.Set(cacheKey, items,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(3)));
                }
            }
            return items;
        }
    }
}

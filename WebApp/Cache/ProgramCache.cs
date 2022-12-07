using Microsoft.Extensions.Caching.Memory;
using WebApp.Models;

namespace WebApp.Cache
{
    public class ProgramCache
    {
        private TvChannelContext db;
        private IMemoryCache cache;
        private int _rowsNumber;

        public ProgramCache(TvChannelContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
        }

        public void Add(string cacheKey)
        {
            IEnumerable<Models.Program> items = db.Programs.Take(_rowsNumber);

            cache.Set(cacheKey, items, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }

        public IEnumerable<Models.Program> Get(string cacheKey)
        {
            IEnumerable<Models.Program> items = null;
            if (!cache.TryGetValue(cacheKey, out items))
            {
                items = db.Programs.Take(_rowsNumber).ToList();
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

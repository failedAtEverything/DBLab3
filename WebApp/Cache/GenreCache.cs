using Microsoft.Extensions.Caching.Memory;
using WebApp.Models;

namespace WebApp.Cache
{
    public class GenreCache
    {
        private TvChannelContext db;
        private IMemoryCache cache;
        private int _rowsNumber;

        public GenreCache(TvChannelContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
        }

        public void Add(string cacheKey)
        {
            IEnumerable<Genre> items = db.Genres.Take(_rowsNumber);

            cache.Set(cacheKey, items, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }

        public IEnumerable<Genre> Get(string cacheKey)
        {
            IEnumerable<Genre> items = null;
            if (!cache.TryGetValue(cacheKey, out items))
            {
                items = db.Genres.Take(_rowsNumber).ToList();
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

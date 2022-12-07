using Microsoft.Extensions.Caching.Memory;
using WebApp.Models;

namespace WebApp.Cache
{
    public class WeeklyCache
    {
        private TvChannelContext db;
        private IMemoryCache cache;
        private int _rowsNumber;

        public WeeklyCache(TvChannelContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
        }

        public void Add(string cacheKey)
        {
            IEnumerable<WeeklyProgramList> items = db.WeeklyProgramLists.Take(_rowsNumber);

            cache.Set(cacheKey, items, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }

        public IEnumerable<WeeklyProgramList> Get(string cacheKey)
        {
            IEnumerable<WeeklyProgramList> items = null;
            if (!cache.TryGetValue(cacheKey, out items))
            {
                items = db.WeeklyProgramLists.Take(_rowsNumber).ToList();
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

using System;
using MeDirect_CurrencyExchangeAPI.IServices;
using MeDirect_CurrencyExchangeAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace MeDirect_CurrencyExchangeAPI.Services
{
    /// <summary>
    /// Implementation of <see cref="ICacheProvider"/> for managing currency-related caching operations.
    /// </summary>

    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;
        private ILogger<CacheProvider> _logger;
        private readonly IConfiguration _configuration;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;
        private ICacheEntry _cacheEntry;
        public CacheProvider(IMemoryCache memoryCache, ILogger<CacheProvider> logger,
                            IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _memoryCache = memoryCache;

            _memoryCacheEntryOptions = new MemoryCacheEntryOptions()
          .SetSlidingExpiration(TimeSpan.FromMinutes(_configuration.GetValue<int>("CacheValues:ExpirationDuration")))

          .SetPriority(CacheItemPriority.High)
          .SetSize(1024);
        }


        /// <summary>
        /// Checks the cache for the presence of <see cref="CurrencyEntity"/> and retrieves it if available.
        /// </summary>
        /// <returns>The cached <see cref="CurrencyEntity"/> if present; otherwise, returns null.</returns>

        public async Task<CurrencyEntity> CheckCache()
        {

            if (_memoryCache.TryGetValue(CacheKeys.CuurencyEntity, out CurrencyEntity currencyEntity))
            {
                _logger.LogInformation("CurrencyEntity retrieved from cache. Cache values are set.");

                return currencyEntity;
            }
            else
            {
                _logger.LogInformation("CurrencyEntity retrieved from cache.");
                return null;

            }
        }

        /// <summary>
        /// Sets the currency cache with the provided <see cref="CurrencyEntity"/> and configures cache entry options.
        /// </summary>
        /// <param name="currencyEntity">The <see cref="CurrencyEntity"/> to be stored in the cache.</param>

        public void SetCurrencyCache(CurrencyEntity currencyEntity)
        {
            // Creating a cache entry for the CurrencyEntity
            _cacheEntry = _memoryCache.CreateEntry(currencyEntity);
            _logger.LogInformation("Cache entry created for CurrencyEntity.");

            // Setting the cache entry with the configured options
            _memoryCache.Set<CurrencyEntity>(CacheKeys.CuurencyEntity, currencyEntity, _memoryCacheEntryOptions);
            _logger.LogInformation($"CurrencyEntity added to cache with expiration duration: {_configuration.GetValue<int>("CacheValues:ExpirationDuration")} minutes.");

        }
    }
}


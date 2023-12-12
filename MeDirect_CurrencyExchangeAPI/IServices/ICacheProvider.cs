using System;
using MeDirect_CurrencyExchangeAPI.Models;

namespace MeDirect_CurrencyExchangeAPI.IServices
{
    /// <summary>
    /// Represents a service for managing currency-related caching operations.
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Sets the currency cache with the provided <paramref name="currencyEntity"/>.
        /// </summary>
        /// <param name="currencyEntity">The currency details to be stored in the cache.</param>

        void SetCurrencyCache(CurrencyEntity currencyEntity);

        /// <summary>
        /// Checks the currency cache and retrieves the cached currency details.
        /// </summary>
        /// <returns>The cached currency details, or null if not found.</returns>

        Task<CurrencyEntity> CheckCache();
    }
}


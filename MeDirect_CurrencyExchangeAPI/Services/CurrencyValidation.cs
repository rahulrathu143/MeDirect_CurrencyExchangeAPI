using System;
using MeDirect_CurrencyExchangeAPI.IServices;
using MeDirect_CurrencyExchangeAPI.Models;

namespace MeDirect_CurrencyExchangeAPI.Services
{
    /// <summary>
    /// Implementation of <see cref="ICurrencyValidation"/> for validating currency pairs.
    /// </summary>

    public class CurrencyValidation:ICurrencyValidation
	{
        private readonly ILogger<CurrencyValidation> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyValidation"/> class.
        /// </summary>
        /// <param name="logger">The logger for logging.</param>

        public CurrencyValidation(ILogger<CurrencyValidation> logger)
		{
            _logger = logger;
		}
        /// <summary>
        /// Checks if the currency pair is valid by ensuring that the base currency matches 'fromCurrency'
        /// and 'toCurrency' is present in the rates dictionary.
        /// </summary>
        /// <param name="currencydetails">The currency details containing the base currency and rates.</param>
        /// <param name="fromCurrency">The source currency in the currency pair.</param>
        /// <param name="toCurrency">The target currency in the currency pair.</param>
        /// <returns>True if the currency pair is valid; otherwise, false.</returns>


        public bool IsValidCurrencyPair(CurrencyEntity currencydetails, string fromCurrency, string toCurrency)
        {
            // Check if the currency pair is valid by ensuring that the base currency matches 'fromCurrency'
            // and 'toCurrency' is present in the rates dictionary.
            return currencydetails.Base.Equals(fromCurrency) && currencydetails.Rates.ContainsKey(toCurrency);
        }
    }
}


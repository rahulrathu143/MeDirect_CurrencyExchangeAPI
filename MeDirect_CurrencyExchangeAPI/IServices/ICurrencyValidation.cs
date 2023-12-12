using System;
using MeDirect_CurrencyExchangeAPI.Models;

namespace MeDirect_CurrencyExchangeAPI.IServices
{

    /// <summary>
    /// Represents a service for validating currency pairs.
    /// </summary>
    public interface ICurrencyValidation
	{
        /// <summary>
        /// Checks whether the specified currency pair is valid.
        /// </summary>
        /// <param name="currencydetails">The currency details containing base and exchange rates.</param>
        /// <param name="fromCurrency">The source currency.</param>
        /// <param name="toCurrency">The target currency.</param>
        /// <returns>True if the currency pair is valid; otherwise, false.</returns>

        bool IsValidCurrencyPair(CurrencyEntity currencydetails, string fromCurrency, string toCurrency);
    }
}


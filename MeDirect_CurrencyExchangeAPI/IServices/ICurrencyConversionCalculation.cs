using System;
using DataAccessLayer;
using MeDirect_CurrencyExchangeAPI.Models;

namespace MeDirect_CurrencyExchangeAPI.IServices
{
    /// <summary>
    /// Represents a service for calculating currency conversion.
    /// </summary>
    public interface ICurrencyConversionCalculation
	{
        /// <summary>
        /// Calculates the currency conversion based on the provided customer details and current exchange rate.
        /// </summary>
        /// <param name="customerEntity">The customer details for currency conversion.</param>
        /// <param name="currentRate">The current exchange rate.</param>
        /// <returns>The resulting customer currency entity after conversion.</returns>

        CutomerCurrencyEntity CalculateCurrencyConversion(CustomerEntity customerEntity, double currentRate);

    }
}


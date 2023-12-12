using System;
using MeDirect_CurrencyExchangeAPI.Models;

namespace MeDirect_CurrencyExchangeAPI.IServices
{
    /// <summary>
    /// Interface defining the contract for fetching currency details from an external API.
    /// </summary>
	public interface ITransactionAPIResponse
	{
        /// <summary>
        /// Fetches currency details from an external API.
        /// </summary>
        /// <returns>The currency details received from the API.</returns>

        Task<CurrencyEntity> FetchCurrencyFromApi();

    }
}


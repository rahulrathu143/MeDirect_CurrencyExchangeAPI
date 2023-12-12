using System;
using DataAccessLayer;
using MeDirect_CurrencyExchangeAPI.DTO;

namespace MeDirect_CurrencyExchangeAPI.IServices
{
    /// <summary>
    /// Represents a service for creating a transaction response.
    /// </summary>
	public interface ICurrencyTransactionResponse
	{
        /// <summary>
        /// Creates a successful transaction response based on the provided customer currency entity.
        /// </summary>
        /// <param name="cutomerCurrencyEntity">The customer currency entity containing transaction details.</param>
        /// <returns>The resulting transaction response.</returns>

        TranscationResponse CreateSuccessResponse(CutomerCurrencyEntity cutomerCurrencyEntity);

    }
}


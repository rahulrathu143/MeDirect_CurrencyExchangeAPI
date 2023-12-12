using System;
using DataAccessLayer;
using MeDirect_CurrencyExchangeAPI.DTO;
using MeDirect_CurrencyExchangeAPI.IServices;

namespace MeDirect_CurrencyExchangeAPI.Services
{
    /// <summary>
    /// Implementation of <see cref="ICurrencyTransactionResponse"/> for creating transaction responses.
    /// </summary>
	public class CurrencyTransactionResponse: ICurrencyTransactionResponse
    {
        ILogger<CurrencyTransactionResponse> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyTransactionResponse"/> class.
        /// </summary>
        /// <param name="logger">The logger for logging.</param>
        public CurrencyTransactionResponse(ILogger<CurrencyTransactionResponse> logger)
		{
            _logger = logger;
		}

        /// <summary>
        /// Creates a successful transaction response with details from 'cutomerCurrencyEntity'.
        /// </summary>
        /// <param name="cutomerCurrencyEntity">The customer currency entity with transaction details.</param>
        /// <returns>The transaction response with status code and details.</returns>

        public TranscationResponse CreateSuccessResponse(CutomerCurrencyEntity cutomerCurrencyEntity)
        {
            // Create a successful transaction response with details from 'cutomerCurrencyEntity'

            _logger.LogInformation($"Transaction successfully processed with customer ID {cutomerCurrencyEntity.CustomerID}.");

            return new TranscationResponse
            {
                StatusCode = StatusCodes.Status201Created,
                FinalAmount = cutomerCurrencyEntity.afterConversionAmount,
                AmountRequested = cutomerCurrencyEntity.amount,
                FromCurrency = cutomerCurrencyEntity.fromCurrency,
                ToCurrency = cutomerCurrencyEntity.toCurrency
            };
        }
    }
}


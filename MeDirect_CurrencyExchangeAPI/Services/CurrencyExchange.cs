using System;
using MeDirect_CurrencyExchangeAPI.Models;
using System.Text.Json;
using System.Linq;
using System.Collections;
using Microsoft.Extensions.Caching.Memory;
using DataAccessLayer.DBOperations;
using DataAccessLayer;
using MeDirect_CurrencyExchangeAPI.IServices;
using AutoMapper;
using MeDirect_CurrencyExchangeAPI.DTO;
using MeDirect_CurrencyExchangeAPI.CustomMiddleware;

namespace MeDirect_CurrencyExchangeAPI.Services
{
    public class CurrencyExchange : ICurrencyExchange
    {
        private readonly IMemoryCache _memoryCache;
        private ILogger<ICurrencyExchange> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICacheProvider _cacheProvider;
        private readonly IMapper _mapper;
        private readonly ITransactionAPIResponse _transactionAPIResponse;
        private readonly ICurrencyTransactionResponse _currencyTransactionResponse;
        private readonly ICurrencyValidation _currencyValidation;
        private readonly ICurrencyConversionCalculation _currencyConversionCalculation;

        public CurrencyExchange(IMemoryCache memoryCache, ILogger<ICurrencyExchange> logger,
                IConfiguration configuration, IHttpClientFactory httpClientFactory,
                ICurrencyRepository currencyRepository, ICacheProvider cacheProvider,
                IMapper mapper, ITransactionAPIResponse transactionAPIResponse,
                ICurrencyTransactionResponse currencyTransactionResponse,
                ICurrencyValidation currencyValidation,
                ICurrencyConversionCalculation currencyConversionCalculation)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _currencyRepository = currencyRepository;
            _cacheProvider = cacheProvider;
            _mapper = mapper;
            _transactionAPIResponse = transactionAPIResponse;
            _currencyTransactionResponse = currencyTransactionResponse;
            _currencyValidation = currencyValidation;
            _currencyConversionCalculation = currencyConversionCalculation;
        }


        #region CurrencyExchangeLogic

        /// <summary>
        /// Processes currency exchange for a customer, validating the currency pair, calculating the converted amount,
        /// and storing the transaction data if the transaction limit is not exceeded.
        /// </summary>
        /// <param name="customerEntity">The customer entity containing currency exchange details.</param>
        /// <returns>The transaction response containing details of the processed currency exchange.</returns>
        /// <exception cref="TransactionLimitException">Thrown when the customer exceeds the allowed transaction limit.</exception>


        public async Task<TranscationResponse> CurrencyExchangeLogic(CustomerEntity customerEntity)
        {
            // Check the currency details in the cache
            CurrencyEntity currencydetails = await GetCurrencyExchangeDetails();

            // Validate if the currency pair is valid
            if (_currencyValidation.IsValidCurrencyPair(currencydetails, customerEntity.fromCurrency, customerEntity.toCurrency))
            {
                // Get the current exchange rate
                var currentRate = currencydetails.Rates[customerEntity.toCurrency];

                // Create a customer currency entity
                // var cutomerCurrencyEntity = CalculateCurrencyConversion(customerEntity, currentRate);

                var cutomerCurrencyEntity = _currencyConversionCalculation.CalculateCurrencyConversion(customerEntity, currentRate);
                //Check the count of customer

                var count = await _currencyRepository.GetCustomerTransactionCount(cutomerCurrencyEntity.CustomerID);
                if (count < 10)//_configuration.GetValue<int>("CustomerThreshold"))
                {
                    // Add transaction data to the repository
                    await _currencyRepository.AddTransactionData(cutomerCurrencyEntity);
                    _logger.LogInformation($"Transaction successfully processed for customer ID {customerEntity.customerID}.");


                    return _currencyTransactionResponse.CreateSuccessResponse(cutomerCurrencyEntity);
                }
                else
                {
                    _logger.LogInformation($"Transaction limit exceeded for customer ID {customerEntity.customerID}.");

                    // Throw a custom exception for exceeding the transaction limit
                    throw new TransactionLimitException();
                }
            }
            else
            {
                _logger.LogInformation($"Invalid currency pair ({customerEntity.fromCurrency} to {customerEntity.toCurrency}).");

                throw new CurrencyNotFound();
            }
        }

        #endregion

        #region fetchCurrencyDetails

        /// <summary>
        /// Retrieves currency details, first attempting to fetch them from the cache,
        /// and if not found, fetches them from the external API, caches the result, and returns the details.
        /// </summary>
        /// <returns>The currency details.</returns>
        /// <exception cref="ApiCommunicationException">Thrown when an unexpected error occurs during API communication.</exception>


        public async Task<CurrencyEntity> GetCurrencyExchangeDetails()
        {
            try
            {
                //Check if currency details are available in the cache
                var currencyEntity = await _cacheProvider.CheckCache();
                if (currencyEntity != null)
                {
                    _logger.LogInformation("Currency details retrieved from cache.");

                    return currencyEntity;
                }
                else
                {
                    // Fetch currency details from the API
                    var currencyDetails = await _transactionAPIResponse.FetchCurrencyFromApi();
                    _cacheProvider.SetCurrencyCache(currencyDetails);
                    _logger.LogInformation("Currency details not found in cache. Fetched from API and cached.");

                    return currencyDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching currency details.");
                throw new ApiCommunicationException();

            }
        }

        #endregion

    }

}
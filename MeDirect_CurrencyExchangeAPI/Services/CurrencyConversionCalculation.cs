using System;
using AutoMapper;
using DataAccessLayer;
using MeDirect_CurrencyExchangeAPI.IServices;
using MeDirect_CurrencyExchangeAPI.Models;

namespace MeDirect_CurrencyExchangeAPI.Services
{
    public class CurrencyConversionCalculation: ICurrencyConversionCalculation
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CurrencyConversionCalculation> _logger;
		public CurrencyConversionCalculation(IMapper mapper, ILogger<CurrencyConversionCalculation> logger)
		{
            _mapper = mapper;
            _logger = logger;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyConversionCalculation"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        /// <param name="logger">The logger for logging.</param>

        public CutomerCurrencyEntity CalculateCurrencyConversion(CustomerEntity customerEntity, double currentRate)
        {

            // Map the properties from 'customerEntity' to 'cutomerCurrencyEntity'
            CutomerCurrencyEntity cutomerCurrencyEntity = _mapper.Map<CutomerCurrencyEntity>(customerEntity);

            // Calculate the converted amount using the provided exchange rate
            cutomerCurrencyEntity.afterConversionAmount = currentRate * customerEntity.amount;

            _logger.LogInformation($"Currency conversion calculated: {customerEntity.amount} {customerEntity.fromCurrency} to {cutomerCurrencyEntity.afterConversionAmount} {cutomerCurrencyEntity.toCurrency}");

            return cutomerCurrencyEntity;
        }

    }
}


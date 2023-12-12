using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.DBOperations;
using MeDirect_CurrencyExchangeAPI;
using MeDirect_CurrencyExchangeAPI.CustomMiddleware;
using MeDirect_CurrencyExchangeAPI.IServices;
using MeDirect_CurrencyExchangeAPI.Models;
using MeDirect_CurrencyExchangeAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MeDirect_CurrencyExchangeAPI_Test
{
    public class CurrencyExchangeTests
    {
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly Mock<ILogger<ICurrencyExchange>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
        private readonly Mock<ICacheProvider> _cacheProviderMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITransactionAPIResponse> _transactionAPIResponseMock;
        private readonly Mock<ICurrencyTransactionResponse> _currencyTransactionResponse;
        private readonly Mock<ICurrencyValidation> _currencyValidation;
        private readonly Mock<ICurrencyConversionCalculation> _currencyConversionCalculation;
        private readonly CurrencyExchange _currencyExchange;
        private readonly Mock<ICurrencyExchange> _mockCurrencyExchange;




        public CurrencyExchangeTests()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
            _loggerMock = new Mock<ILogger<ICurrencyExchange>>();
            _configurationMock = new Mock<IConfiguration>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _currencyRepositoryMock = new Mock<ICurrencyRepository>();
            _cacheProviderMock = new Mock<ICacheProvider>();
            _mapperMock = new Mock<IMapper>();
            _transactionAPIResponseMock = new Mock<ITransactionAPIResponse>();
            _currencyTransactionResponse = new Mock<ICurrencyTransactionResponse>();
            _currencyValidation = new Mock<ICurrencyValidation>();
            _currencyConversionCalculation = new Mock<ICurrencyConversionCalculation>();
            _mockCurrencyExchange = new Mock<ICurrencyExchange>();

            _currencyExchange = new CurrencyExchange(
                _memoryCacheMock.Object,
                _loggerMock.Object,
                _configurationMock.Object,
                _httpClientFactoryMock.Object,
                _currencyRepositoryMock.Object,
                _cacheProviderMock.Object,
                _mapperMock.Object,
                _transactionAPIResponseMock.Object,
                _currencyTransactionResponse.Object,
                _currencyValidation.Object,
                _currencyConversionCalculation.Object
            );
        }


        

        [Fact]
        public async Task CurrencyExchangeLogic_ValidCurrencyPair_SuccessfulTransaction()
        {
            // Arrange
            var currencyDetails = new CurrencyEntity
            {
                Base = "USD",
                Rates = new Dictionary<string, double>
            {
                { "EUR", 0.85 },
                { "GBP", 0.75 }
            }
            };
             _cacheProviderMock.Setup(cp => cp.CheckCache()).ReturnsAsync(new CurrencyEntity
            {
                Success = true,
                Timestamp = 1234567890,
                Date = DateTime.Now,
                Base = "USD",
                Rates = new Dictionary<string, double>
                {
                    { "EUR", 0.85 },
                    { "GBP", 0.75 },
                  
                }
            });
            var currencyValidationMock = new Mock<ICurrencyValidation>();
            currencyValidationMock.Setup(v => v.IsValidCurrencyPair(It.IsAny<CurrencyEntity>(), It.IsAny<string>(), It.IsAny<string>()))
                                  .Returns(true);

            var currencyConversionCalculationMock = new Mock<ICurrencyConversionCalculation>();
            currencyConversionCalculationMock.Setup(c => c.CalculateCurrencyConversion(It.IsAny<CustomerEntity>(), It.IsAny<double>()))
                                            .Returns(new CutomerCurrencyEntity
                                            {
                                                CustomerID = 123,
                                                afterConversionAmount = 50.0

                                            });

            var currencyRepositoryMock = new Mock<ICurrencyRepository>();
            currencyRepositoryMock.Setup(r => r.GetCustomerTransactionCount(It.IsAny<long>()))
                                  .ReturnsAsync(0);


            var loggerMock = new Mock<ILogger<ICurrencyExchange>>();
            var currencyTransactionResponseMock = new Mock<ICurrencyTransactionResponse>();



            // Act
            var result = await _currencyExchange.CurrencyExchangeLogic(new CustomerEntity
            {
                fromCurrency = "USD",
                toCurrency = "EUR",
                // Add other necessary properties
            });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Equal(50.0, result.FinalAmount);
            // Add other necessary assertions
        }
    

        

        [Fact]
        public async Task GetCurrencyDetails_Cache_Is_Not_Empty()
        {
            // Arrange

            // Mock the response of CheckCache method
            var currencyDetails = _cacheProviderMock.Setup(cp => cp.CheckCache()).ReturnsAsync(new CurrencyEntity
            {
                Success = true,
                Timestamp = 1234567890,
                Date = DateTime.Now,
                Base = "USD",
                Rates = new Dictionary<string, double>
                {
                    { "EUR", 0.85 },
                    { "GBP", 0.75 },
                    // Add other currency rates as needed
                }
            });

            CurrencyEntity currencyEntity = new CurrencyEntity()
            {
                Success = true,
                Timestamp = 1234567890,
                Date = DateTime.Now,
                Base = "USD",
                Rates = new Dictionary<string, double>
                {
                    { "EUR", 0.85 },
                    { "GBP", 0.75 } }
            };



            // Mock the response of GetCustomerTransactionCount method
            //currencyRepositoryMock.Setup(cr => cr.GetCustomerTransactionCount(It.IsAny<long>())).ReturnsAsync(0);

            // Mock the response of AddTransactionData method
            //currencyRepositoryMock.Setup(cr => cr.AddTransactionData(It.IsAny<CutomerCurrencyEntity>())).Returns(Task.CompletedTask);

            //Mock the response of FetchCurrencyFromApi method

            // Act

            var result = await _currencyExchange.GetCurrencyExchangeDetails();

            //Assert
            Assert.Equal(currencyEntity.Base, result.Base);
            _cacheProviderMock.Verify(c => c.CheckCache(), Times.Once);
            _transactionAPIResponseMock.Verify(t => t.FetchCurrencyFromApi(), Times.Never);

        }


        [Fact]
        public async Task GetCurrencyDetails_Cache_Is_Empty_Call_FetchAPIREsponse()
        {

            CurrencyEntity currencyEntity = new CurrencyEntity()
            {
                Success = true,
                Timestamp = 1234567890,
                Date = DateTime.Now,
                Base = "USD",
                Rates = new Dictionary<string, double>
                {
                    { "EUR", 0.85 },
                    { "GBP", 0.75 } }
            };

            var apiResponse = _transactionAPIResponseMock.Setup(cp => cp.FetchCurrencyFromApi()).ReturnsAsync(currencyEntity);

            // Act

            var result = await _currencyExchange.GetCurrencyExchangeDetails();

            //Assert
            Assert.Equal(currencyEntity.Base, result.Base);
            _cacheProviderMock.Verify(c => c.CheckCache(), Times.Once);
            _transactionAPIResponseMock.Verify(t => t.FetchCurrencyFromApi(), Times.Once);

        }
        // Add more test methods for other scenarios
    }
}


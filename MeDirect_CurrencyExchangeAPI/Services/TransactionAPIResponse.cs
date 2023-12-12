using System;
using System.Net.Http;
using System.Text.Json;
using MeDirect_CurrencyExchangeAPI.IServices;
using MeDirect_CurrencyExchangeAPI.Models;
using Microsoft.Extensions.Configuration;

namespace MeDirect_CurrencyExchangeAPI.Services
{
    /// <summary>
    /// Implementation of the <see cref="ITransactionAPIResponse"/> interface for fetching currency details from an external API.
    /// </summary>
    public class TransactionAPIResponse : ITransactionAPIResponse
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TransactionAPIResponse> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Constructor for initializing the TransactionAPIResponse.
        /// </summary>
        /// <param name="configuration">The configuration for accessing application settings.</param>
        /// <param name="logger">The logger for logging messages.</param>
        /// <param name="httpClientFactory">The factory for creating HttpClient instances.</param>

        public TransactionAPIResponse(IConfiguration configuration, ILogger<TransactionAPIResponse> logger,
                                       IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Fetches currency details from an external API.
        /// </summary>
        /// <returns>The currency details received from the API.</returns>

        public async Task<CurrencyEntity> FetchCurrencyFromApi()
        {
            // Construct the URL for the currency API
            var url = string.Format(_configuration.GetValue<string>("CurrencyAPI:BaseAddress") +
                                    _configuration.GetValue<string>("CurrencyAPI:AccessKey"));

            // Create an HttpClient using the named client "CurrencyExchangeAPI" defined in program.cs
            var httpClient = _httpClientFactory.CreateClient("CurrencyExchangeAPI");

            // Make a request to the currency API
            var httpResponseMessage = await httpClient.GetAsync(
            _configuration.GetValue<string>("CurrencyAPI:APIEndPoint") +
                _configuration.GetValue<string>("CurrencyAPI:AccessKey"));

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var stringResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                // Deserialize the string response into a CurrencyEntity object
                return JsonSerializer.Deserialize<CurrencyEntity>(stringResponse);
            }
            else
            {
                // Log an error when the API request is not successful

                // Throw an exception with the reason phrase
                throw new HttpRequestException(httpResponseMessage.ReasonPhrase);
            }
        }
    }
}

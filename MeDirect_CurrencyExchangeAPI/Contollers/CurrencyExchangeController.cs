using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.DBOperations;
using MeDirect_CurrencyExchangeAPI.DTO;
using MeDirect_CurrencyExchangeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MeDirect_CurrencyExchangeAPI.Contollers
{

    [Route("api/[controller]")]
    //[ApiVersion("1.0")]
    //versioning
    public class CurrencyExchangeController : Controller
    {

        private readonly ICurrencyExchange _currencyExchange;
        private readonly ILogger<CurrencyExchangeController> _logger;

        public CurrencyExchangeController(ICurrencyExchange currencyExchange,ILogger<CurrencyExchangeController> logger)
        {
            _currencyExchange = currencyExchange;
            _logger = logger;

        }

        /// <summary>
        /// Retrieves currency exchange details using the Currency Exchange service.
        /// </summary>
        /// <returns>An HTTP action result containing the currency exchange details.</returns>

        [HttpGet]
        public async Task<IActionResult> CurrencyExchanger() =>
                Ok(await _currencyExchange.GetCurrencyExchangeDetails());



        /// <summary>
        /// Processes customer details and performs currency exchange logic.
        /// </summary>
        /// <param name="customerEntity">The customer details for currency exchange.</param>
        /// <returns>An HTTP action result containing the transaction response.</returns>


        [HttpPost]
        public async Task<IActionResult> SetDetails([FromBody] CustomerEntity customerEntity)
        {
            if (!ModelState.IsValid || customerEntity.customerID==0 || customerEntity.amount ==0)
            {
                _logger.LogInformation("Invalid model state for upcoming request.");
                return BadRequest("Enter Valid Data");
            }
            var transcationResponse = await _currencyExchange.CurrencyExchangeLogic(customerEntity);
            _logger.LogInformation($"Currency exchange logic processed successfully for customer ID {customerEntity.customerID}.");
            return Ok(transcationResponse);

        }


    }
}


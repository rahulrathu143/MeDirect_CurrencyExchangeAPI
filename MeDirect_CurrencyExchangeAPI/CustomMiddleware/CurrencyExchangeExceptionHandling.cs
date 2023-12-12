using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MeDirect_CurrencyExchangeAPI.CustomMiddleware
{
    public class CurrencyExchangeExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CurrencyExchangeExceptionHandling> _logger;

        public CurrencyExchangeExceptionHandling(RequestDelegate next, ILogger<CurrencyExchangeExceptionHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                HandleException(context, ex, HttpStatusCode.NotFound, "Resource not found.");
            }
            catch (CurrencyNotFound ex)
            {
                HandleException(context, ex, HttpStatusCode.BadRequest, "Currency not found in the list.Please provide appropriate currency");
            }
            catch (ApiCommunicationException ex)
            {
                HandleException(context, ex, HttpStatusCode.InternalServerError, "Sorry, an unexpected error occurred while communicating with the external service. Please try again later.");
            }
            catch (TransactionLimitException ex)
            {
                HandleException(context, ex, HttpStatusCode.Forbidden, "Transaction limit exhausted.Please try after sometime");
            }
            catch (HttpRequestException ex)
            {
                HandleException(context, ex, HttpStatusCode.NotFound, "Third party API not available .Please try again after sometime");
            }
            catch (DbUpdateException ex)
            {
                HandleException(context, ex, HttpStatusCode.NotFound, "Third party API not available .Please try again after sometime");
            }
            catch (Exception ex)
            {
                HandleException(context, ex, HttpStatusCode.InternalServerError, "An unexpected error occurred.Please try again after sometime");
            }
        }

        private void HandleException(HttpContext context, Exception ex, HttpStatusCode statusCode, string errorMessage)
        {
            _logger.LogError(ex, errorMessage);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = errorMessage });
            context.Response.WriteAsync(result);
        }
    }

    public class NotFoundException : Exception
    {
        // Custom exception for resource not found
    }


    public class CurrencyNotFound : Exception
    {
        // Custom exception for Currency not found
    }
    public class TransactionLimitException : Exception
    {
        // Custom exception for Transaction Limit exhausted 
    }
    public class ApiCommunicationException : Exception
    {

    }
}
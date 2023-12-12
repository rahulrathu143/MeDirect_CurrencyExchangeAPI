using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.DBOperations
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CustomerCurrencyDbContext _customerCurrencyDbContext;
        private readonly ILogger<CurrencyRepository> _logger;
        public CurrencyRepository(CustomerCurrencyDbContext customerCurrencyDbContext,
                                   ILogger<CurrencyRepository> logger)
        {
            _customerCurrencyDbContext = customerCurrencyDbContext;
            _logger = logger;
        }

        /// <summary>
        /// Adds a transaction record to the database for a customer currency entity and saves changes.
        /// </summary>
        /// <param name="entity">The customer currency entity representing the transaction.</param>
        /// <exception cref="DbUpdateException">Thrown when an error occurs during the database update operation.</exception>
        /// <exception cref="Exception">Thrown for general database-related errors.</exception>


        public async Task AddTransactionData(CutomerCurrencyEntity entity)
        {
            try
            {
                // Add the customer currency entity to the DbContext
                _customerCurrencyDbContext.cutomerCurrencyEntities.Add(entity);


                // Save changes to the database
                await _customerCurrencyDbContext.SaveChangesAsync();
                _logger.LogInformation("Customer data added and  saved to the database successfully.");
            }

            catch (DbUpdateException e)
            {
                // Log an error when data is not saved in the database
                _logger.LogError("Database Entry failed", e.StackTrace);

                // Rethrow the exception for handling or propagation which is handled in the CurrencyExchangeExceptionHandling
                throw new Exception();
            }
            catch (Exception e)
            {
                // Log a generic error for other database-related exceptions
                _logger.LogError("Database calling failed", e.StackTrace);

                // Rethrow the exception for handling and appropriate statuscode and message will be sent back
                throw new Exception();
            }
        }


        /// <summary>
        /// Retrieves the count of transactions for a specific customer within the last hour from the database.
        /// </summary>
        /// <param name="CustomerID">The unique identifier of the customer.</param>
        /// <returns>The count of transactions for the specified customer within the last hour.</returns>
        /// <exception cref="DbUpdateException">Thrown when an error occurs during the database update operation.</exception>
        /// <exception cref="Exception">Thrown for general database-related errors.</exception>

        public async Task<int> GetCustomerTransactionCount(long CustomerID)
        {
            try
            {
                // Query the database for transactions within the last hour for the specified customer

                var count = await _customerCurrencyDbContext.cutomerCurrencyEntities.Where<CutomerCurrencyEntity>
                        (propg => propg.CustomerID == CustomerID & propg.TranscationTimestamp
                               >= DateTime.Now.AddHours(-1)).ToListAsync();
                // Log success message
                _logger.LogInformation("Database retreived suceefully");

                // Return the count of transactions occured in last one hour
                return count.Count();
            }

            catch (DbUpdateException e)
            {
                // Log an error for a database update exception
                _logger.LogError("Error occurred while retreiving data from Database query", e.StackTrace);

                // Rethrow the exception for handling and appropriate statuscode and message will be sent back
                throw new Exception(e.StackTrace);
            }

            catch (Exception e)
            {
                // Log an error for a database update exception
                _logger.LogError("Error occurred while retreiving data from Database query", e.StackTrace);

                // Rethrow the exception for handling and appropriate statuscode and message will be sent back
                throw new Exception(e.StackTrace);
            }

        }
    }
}


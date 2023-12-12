using System;
namespace DataAccessLayer.DBOperations
{
    public interface ICurrencyRepository
    {
        /// <summary>
        /// Adds a transaction record to the repository.
        /// </summary>
        /// <param name="entity">The customer currency entity representing the transaction.</param>
        /// <returns>A task representing the asynchronous operation.</returns>

        Task AddTransactionData(CutomerCurrencyEntity entity);

        /// <summary>
        /// Retrieves the count of transactions for a specific customer within the last hour.
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer.</param>
        /// <returns>The count of transactions for the specified customer within the last hour.</returns>
        /// <returns>A task representing the asynchronous operation.</returns>

        Task<int> GetCustomerTransactionCount(long CustomerID);
    }
}


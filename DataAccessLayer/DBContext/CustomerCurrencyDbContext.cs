using System;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class CustomerCurrencyDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerCurrencyDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the context.</param>

        public CustomerCurrencyDbContext(DbContextOptions options)
            : base(options)
        {

        }

        /// <summary>
        /// Gets or sets the DbSet for customer currency entities.
        /// </summary>
        public DbSet<CutomerCurrencyEntity>? cutomerCurrencyEntities { get; set; }
    }
}



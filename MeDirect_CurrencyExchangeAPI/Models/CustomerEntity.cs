using System;
namespace MeDirect_CurrencyExchangeAPI.Models
{
    public class CustomerEntity
    {
        public long customerID { get; set; }
        public string fromCurrency { get; set; }
        public string toCurrency { get; set; }
        public double amount { get; set; }
    }
}


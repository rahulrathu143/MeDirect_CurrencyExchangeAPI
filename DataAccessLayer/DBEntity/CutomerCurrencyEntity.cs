using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer
{
	public class CutomerCurrencyEntity
	{
        [Key]
        public long CustomerTransactionID { get; set; }
        public long CustomerID { get; set;}
        public string? fromCurrency { get; set; }
        public string? toCurrency { get; set; }
        public double amount { get; set; }
        
        public double afterConversionAmount { get; set; }
        public DateTime TranscationTimestamp { get; set; } = DateTime.Now;
    
	}
}


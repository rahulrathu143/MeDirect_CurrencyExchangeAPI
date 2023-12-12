using System;
namespace MeDirect_CurrencyExchangeAPI.DTO
{
    public class TranscationResponse
    {
        public double FinalAmount { get; set; }
        public int StatusCode { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public double AmountRequested { get; set; }
    }
}


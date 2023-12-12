using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MeDirect_CurrencyExchangeAPI.Models
{
    public class CurrencyEntity
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("timestamp")]
        public double Timestamp { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("base")]
        public string? Base { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, double>? Rates { get; set; }

    }
}


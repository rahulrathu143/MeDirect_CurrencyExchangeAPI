using System;
using System.Collections;
using DataAccessLayer;
using MeDirect_CurrencyExchangeAPI.DTO;
using MeDirect_CurrencyExchangeAPI.Models;

namespace MeDirect_CurrencyExchangeAPI
{

    public interface ICurrencyExchange
    {
        Task<CurrencyEntity> GetCurrencyExchangeDetails();
        Task<TranscationResponse> CurrencyExchangeLogic(CustomerEntity customerEntity);
    }

}


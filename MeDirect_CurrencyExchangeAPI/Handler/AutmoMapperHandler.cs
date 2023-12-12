using System;
using AutoMapper;
using DataAccessLayer;
using MeDirect_CurrencyExchangeAPI.DTO;
using MeDirect_CurrencyExchangeAPI.Models;

namespace MeDirect_CurrencyExchangeAPI.Handler
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<CustomerEntity, CutomerCurrencyEntity>();
        }
    }
}


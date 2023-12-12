using DataAccessLayer;
using DataAccessLayer.DBOperations;
using MeDirect_CurrencyExchangeAPI;
using MeDirect_CurrencyExchangeAPI.IServices;
using MeDirect_CurrencyExchangeAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using AutoMapper;
using MeDirect_CurrencyExchangeAPI.Handler;
using MeDirect_CurrencyExchangeAPI.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper for object-object mapping
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add HttpClient named "CurrencyExchangeAPI" with a base address
builder.Services.AddHttpClient("CurrencyExchangeAPI", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["CurrencyAPI:BaseAddress"]);
});

// Adding in-memory caching
builder.Services.AddMemoryCache();

// Add DbContext for CustomerCurrencyDbContext using SQL Server
builder.Services.AddDbContext<CustomerCurrencyDbContext>(opts => opts.UseSqlServer(builder.Configuration["ConnectionString:CustomerDB"]));

// Add scoped services for repositories and services
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ICurrencyExchange, CurrencyExchange>();

// Add singleton service for cache provider
builder.Services.AddSingleton<ICacheProvider, CacheProvider>();
builder.Services.AddSingleton<ITransactionAPIResponse, TransactionAPIResponse>();
builder.Services.AddSingleton<ICurrencyTransactionResponse,CurrencyTransactionResponse>();
builder.Services.AddSingleton<ICurrencyValidation, CurrencyValidation>();
builder.Services.AddSingleton<ICurrencyConversionCalculation, CurrencyConversionCalculation>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.UseMiddleware<CurrencyExchangeExceptionHandling>();
app.MapControllers();

app.Run();

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteWipro.Common;
using TesteWipro.Data;
using TesteWipro.Data.Impl;
using TesteWipro.Logic.Models;
using TesteWipro.Logic.Services;
using TesteWipro.Logic.Services.Impl;
using Xunit;

namespace TesteWipro.Tests
{
    public class QueueTests
    {
        private ICurrencyServices _currencyServices;
        private IQueueContextFactory _contextFactory;

        public QueueTests() 
        {
            this._contextFactory = new InMemoryQueueContextFactory();
            this._currencyServices = new CurrencyServices(this._contextFactory);
        }

        [Fact]
        public async void ShouldAddCurrencyToQueue()
        {
            List<Currency> currency = await this._currencyServices.AddCurrenciesToProcessingQueue(new List<Currency>() 
            { 
                new Currency 
                {
            
                    moeda = "Testing currency",
                    data_inicio = DateTime.Now,
                    data_fim = DateTime.Now.AddDays(1)
            
                }
            });

            Assert.NotNull(currency);
        }

        [Fact]
        public async Task ShouldNotAddEmptyCurrency()
        {
            var exception = await Assert.ThrowsAsync<BusinessException>(() => this._currencyServices.AddCurrenciesToProcessingQueue(new List<Currency>() { null }));

            Assert.IsType<BusinessException>(exception);
            Assert.Equal("Line 1 - Currency data cannot be null", exception.Message);
        }

        [Fact]
        public async Task ShouldNotAddCurrencyWithStartDateAfterEndDate()
        {
            var currencies = new List<Currency>()
            {
                new Currency
                {

                    moeda = "Testing currency",
                    data_inicio = DateTime.Now.AddDays(1),
                    data_fim = DateTime.Now

                }
            };

            var exception = await Assert.ThrowsAsync<BusinessException>(() => this._currencyServices.AddCurrenciesToProcessingQueue(currencies));

            Assert.IsType<BusinessException>(exception);
            Assert.Equal("Line 1 - Start date cannot be after end date", exception.Message);
        }

        [Fact]
        public async Task ShouldNotAddCurrencyWithoutAName()
        {
            var currencies = new List<Currency>()
            {
                new Currency
                {

                    moeda = "",
                    data_inicio = DateTime.Now,
                    data_fim = DateTime.Now.AddDays(1)

                }
            };

            var exception = await Assert.ThrowsAsync<BusinessException>(() => this._currencyServices.AddCurrenciesToProcessingQueue(currencies));

            Assert.IsType<BusinessException>(exception);
            Assert.Equal("Line 1 - Currency name cannot be empty", exception.Message);
        }
    }
}

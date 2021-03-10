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
            Currency currency = await this._currencyServices.AddCurrencyToProcessingQueue(new Currency
            {
                Name = "Testing currency",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            });

            Assert.NotNull(currency);
        }

        [Fact]
        public async Task ShouldNotAddEmptyCurrency()
        {
            var exception = await Assert.ThrowsAsync<BusinessException>(() => this._currencyServices.AddCurrencyToProcessingQueue(null));

            Assert.IsType<BusinessException>(exception);
            Assert.Equal("Currency data cannot be null", exception.Message);
        }

        [Fact]
        public async Task ShouldNotAddCurrencyWithStartDateAfterEndDate()
        {
            var currency = new Currency()
            {
                Name = "Testing currency",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now
            };

            var exception = await Assert.ThrowsAsync<BusinessException>(() => this._currencyServices.AddCurrencyToProcessingQueue(currency));

            Assert.IsType<BusinessException>(exception);
            Assert.Equal("Start date cannot be after end date", exception.Message);
        }

        [Fact]
        public async Task ShouldNotAddCurrencyWithoutAName()
        {
            var currency = new Currency()
            {
                Name = "",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            };

            var exception = await Assert.ThrowsAsync<BusinessException>(() => this._currencyServices.AddCurrencyToProcessingQueue(currency));

            Assert.IsType<BusinessException>(exception);
            Assert.Equal("Currency name cannot be empty", exception.Message);
        }
    }
}

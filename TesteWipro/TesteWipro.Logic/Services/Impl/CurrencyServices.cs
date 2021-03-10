using TesteWipro.Data;
using TesteWipro.Data.Models;
using TesteWipro.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using TesteWipro.Common;

namespace TesteWipro.Logic.Services.Impl
{
    public class CurrencyServices : ICurrencyServices
    {
        private IQueueContextFactory _queueContextFactory;

        public CurrencyServices(IQueueContextFactory queueContextFactory) 
        {
            this._queueContextFactory = queueContextFactory;
        }

        public async Task<Currency> AddCurrencyToProcessingQueue(Currency currency)
        {
            IQueue<CurrencyModel> queue = this.GetQueue();

            this.ValidateCurrency(currency);

            CurrencyModel created = await queue.Enqueue(this.ConvertCurrencyToCurrencyModel(currency));

            return this.ConvertCurrencyModelToCurrency(created);
        }

        public async Task<Currency> GetCurrencyFromQueue()
        {

            IQueue<CurrencyModel> queue = this.GetQueue();

            CurrencyModel nextCurrency = await queue.Dequeue();

            return this.ConvertCurrencyModelToCurrency(nextCurrency);
        }

        private IQueue<CurrencyModel> GetQueue()
        {
            IQueueContext queueContext = this._queueContextFactory.CreateNew();
            IQueue<CurrencyModel> queue = queueContext.GetQueue<CurrencyModel>();

            return queue;
        }

        private Currency ConvertCurrencyModelToCurrency(CurrencyModel currency)
        {
            if (currency == null)
                return null;

            return new Currency
            {
                EndDate = currency.EndDate,
                Name = currency.Name,
                StartDate = currency.StartDate
            };
        }

        private CurrencyModel ConvertCurrencyToCurrencyModel(Currency currency)
        {
            if (currency == null)
                return null;

            return new CurrencyModel
            {
                Id = Guid.NewGuid(),
                EndDate = currency.EndDate,
                StartDate = currency.StartDate,
                Name = currency.Name
            };
        }

        private void ValidateCurrency(Currency currency) 
        {
            if (currency == null) 
            {
                throw new BusinessException("Currency data cannot be null");
            }

            if (string.IsNullOrEmpty(currency.Name))
            {
                throw new BusinessException("Currency name cannot be empty");
            }

            if (currency.StartDate == null) 
            {
                throw new BusinessException("Start date cannot be null");
            }

            if (currency.EndDate == null)
            {
                throw new BusinessException("End date cannot be null");
            }

            if (currency.StartDate > currency.EndDate)
            {
                throw new BusinessException("Start date cannot be after end date");
            }

            if (currency.StartDate.Date == currency.EndDate.Date)
            {
                throw new BusinessException("Start date cannot be the same as end date");
            }
        }
    }
}

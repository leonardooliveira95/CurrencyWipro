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

        public async Task<List<Currency>> AddCurrenciesToProcessingQueue(List<Currency> currencies)
        {
            IQueue<List<CurrencyModel>> queue = this.GetQueue();

            int index = 1;
            foreach (Currency currency in currencies) 
            {
                this.ValidateCurrency(currency, index);

                index++;
            }

            List<CurrencyModel> created = await queue.Enqueue(currencies.Select(currency => this.ConvertCurrencyToCurrencyModel(currency)).ToList());

            return created.Select(currencyModel => this.ConvertCurrencyModelToCurrency(currencyModel)).ToList();
        }

        public async Task<List<Currency>> GetCurrenciesFromQueue()
        {

            IQueue<List<CurrencyModel>> queue = this.GetQueue();

            List<CurrencyModel> nextCurrencies = await queue.Dequeue();

            if (nextCurrencies == null)
                return new List<Currency>();

            return nextCurrencies.Select(x => this.ConvertCurrencyModelToCurrency(x)).ToList();
        }

        private IQueue<List<CurrencyModel>> GetQueue()
        {
            IQueueContext queueContext = this._queueContextFactory.CreateNew();
            IQueue<List<CurrencyModel>> queue = queueContext.GetQueue<List<CurrencyModel>>();

            return queue;
        }

        private Currency ConvertCurrencyModelToCurrency(CurrencyModel currency)
        {
            if (currency == null)
                return null;

            return new Currency
            {
                data_fim = currency.EndDate,
                moeda = currency.Name,
                data_inicio = currency.StartDate
            };
        }

        private CurrencyModel ConvertCurrencyToCurrencyModel(Currency currency)
        {
            if (currency == null)
                return null;

            return new CurrencyModel
            {
                Id = Guid.NewGuid(),
                EndDate = currency.data_fim,
                StartDate = currency.data_inicio,
                Name = currency.moeda
            };
        }

        private void ValidateCurrency(Currency currency, int index) 
        {
            if (currency == null) 
            {
                throw new BusinessException($"Line {index} - Currency data cannot be null");
            }

            if (string.IsNullOrEmpty(currency.moeda))
            {
                throw new BusinessException($"Line {index} - Currency name cannot be empty");
            }

            if (currency.data_inicio == null) 
            {
                throw new BusinessException($"Line {index} - Start date cannot be null");
            }

            if (currency.data_fim == null)
            {
                throw new BusinessException($"Line {index} - End date cannot be null");
            }

            if (currency.data_inicio > currency.data_fim)
            {
                throw new BusinessException($"Line {index} - Start date cannot be after end date");
            }

            if (currency.data_inicio.Date == currency.data_fim.Date)
            {
                throw new BusinessException($"Line {index} - Start date cannot be the same as end date");
            }
        }
    }
}

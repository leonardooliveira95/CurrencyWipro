using TesteWipro.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TesteWipro.Logic.Services
{
    public interface ICurrencyServices
    {
        Task<Currency> GetCurrencyFromQueue();
        Task<Currency> AddCurrencyToProcessingQueue(Currency currency);
    }
}

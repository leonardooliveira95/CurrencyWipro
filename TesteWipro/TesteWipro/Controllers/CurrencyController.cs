using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteWipro.Common;
using TesteWipro.Logic.Models;
using TesteWipro.Logic.Services;

namespace TesteWipro.Controllers
{
    /// <summary>
    /// Controller for fetching and placing items on a queue
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private ICurrencyServices _currencyServices;

        public CurrencyController(ICurrencyServices currencyServices)
        {
            this._currencyServices = currencyServices;
        }

        /// <summary>
        /// Gets the next currency on the queue
        /// </summary>
        /// <returns> Next currency on the queue </returns>
        [HttpGet]
        [Route("GetItemFila")]
        public async Task<Currency> GetItemFila()
        {
            Currency result = await _currencyServices.GetCurrencyFromQueue();
            return result;
        }

        /// <summary>
        /// Add currency to the queue
        /// </summary>
        /// <returns> Added currency </returns>
        [HttpPost]
        [Route("AddItemFila")]
        public async Task<IActionResult> AddItemFila([FromBody] Currency currency)
        {
            try
            {
                Currency result = await _currencyServices.AddCurrencyToProcessingQueue(currency);
                return Ok(result);
            }
            catch (BusinessException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

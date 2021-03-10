using System;
using System.Collections.Generic;
using System.Text;

namespace TesteWiproClient
{
    public class CurrencyPriceData
    {
        public decimal Price { get; set; }
        public CurrencyType Type { get; set; }
        public DateTime Date { get; set; }
    }
}

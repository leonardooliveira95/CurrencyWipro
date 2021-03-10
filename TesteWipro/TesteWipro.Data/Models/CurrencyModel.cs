using System;
using System.Collections.Generic;
using System.Text;

namespace TesteWipro.Data.Models
{
    public class CurrencyModel : AbstractModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

using TesteWipro.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteWipro.Data
{
    public interface IQueueContext
    {
        IQueue<TModel> GetQueue<TModel>();
    }
}

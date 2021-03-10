using TesteWipro.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteWipro.Data.Impl
{
    public class DatabaseQueueContext : IQueueContext
    {
        public IQueue<TModel> GetQueue<TModel>() where TModel : AbstractModel
        {
            //Example stub implementation of a queue that could be saved/retrieved from a database
            //Additional logic for creating the queue could be set here, like logging, localizers and services
            return new DatabaseQueue<TModel>();
        }
    }
}

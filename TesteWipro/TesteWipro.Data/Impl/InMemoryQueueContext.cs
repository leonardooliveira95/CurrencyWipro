using TesteWipro.Data.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TesteWipro.Data.Impl
{
    public class InMemoryQueueContext : IQueueContext
    {
        private ConcurrentDictionary<string, object> _queues;

        public InMemoryQueueContext() 
        {
            this._queues = new ConcurrentDictionary<string, object>();
        }

        public IQueue<TModel> GetQueue<TModel>()
        {
            Type type = typeof(TModel);

            InMemoryQueue<TModel> selectedQueue = (InMemoryQueue<TModel>)this._queues.GetOrAdd(type.FullName, new InMemoryQueue<TModel>());

            return selectedQueue;
        }
    }
}

using TesteWipro.Data.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace TesteWipro.Data.Impl
{
    public class InMemoryQueue<TModel> : IQueue<TModel>
    {
        private readonly ConcurrentQueue<TModel> _queue;

        public InMemoryQueue() 
        {
            this._queue = new ConcurrentQueue<TModel>();
        }

        public Task<TModel> Enqueue(TModel item)
        {
            this._queue.Enqueue(item);
            return Task.FromResult(item);
        }

        public Task<TModel> Dequeue()
        {
            TModel item = default;

            if (this._queue.Count() == 0) 
            {
                return Task.FromResult(item);
            }

            if (!this._queue.TryDequeue(out item))
            {
                throw new Exception("Unexpected error while removing item from queue");
            }
            else 
            {
                return Task.FromResult(item);
            }
        }

        public Task<IEnumerable<TModel>> List()
        {
            List<TModel> result = new List<TModel>();

            foreach (TModel model in _queue) 
            {
                result.Add(model);
            }

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<TModel> Peek()
        {
            TModel item = default;

            if (this._queue.Count() == 0)
            {
                return Task.FromResult(item);
            }

            if (!this._queue.TryPeek(out item))
            {
                throw new Exception("Unexpected error while removing item from queue");
            }
            else
            {
                return Task.FromResult(item);
            }
        }

        public Task<int> Count()
        {
            return Task.FromResult(this._queue.Count());
        }
    }
}

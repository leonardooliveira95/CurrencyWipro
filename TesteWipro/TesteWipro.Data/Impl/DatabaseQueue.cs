using TesteWipro.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TesteWipro.Data.Impl
{
    //Example implementation of a queue that could be saved/retrieved from a database
    public class DatabaseQueue<TModel> : IQueue<TModel>
    {
        public Task<TModel> Enqueue(TModel item)
        {
            throw new NotImplementedException();
        }

        public Task<TModel> Dequeue()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TModel>> List()
        {
            throw new NotImplementedException();
        }

        public Task<TModel> Peek()
        {
            throw new NotImplementedException();
        }

        public Task<int> Count()
        {
            throw new NotImplementedException();
        }
    }
}

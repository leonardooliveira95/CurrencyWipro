using System;
using System.Collections.Generic;
using System.Text;

namespace TesteWipro.Data
{
    public interface IQueueContextFactory
    {
        IQueueContext CreateNew();
    }
}

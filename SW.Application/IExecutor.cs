using SW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Application
{
    interface IExecutor<T>
    {
        ICollection<T> Execute();

        T GetByName(string name);
    }
}

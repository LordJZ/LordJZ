using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ
{
    public interface IObjectProxy
    {
        object ProxifiedObject { get; }
    }

    public interface IObjectProxy<out T> : IObjectProxy
    {
        new T ProxifiedObject { get; }
    }
}

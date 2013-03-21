using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public interface IOlapDataVector
    {
        object this[int index] { get; }
        T Get<T>(int index);
    }
}

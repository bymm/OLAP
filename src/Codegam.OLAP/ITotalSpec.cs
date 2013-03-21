using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public interface ITotalSpec
    {
        ShowTotal ShowTotal { get; }
        string TotalName { get; }
    }
}

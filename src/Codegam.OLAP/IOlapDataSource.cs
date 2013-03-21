using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public interface IOlapDataSource : IEnumerable<IOlapDataVector>
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public interface IGroup : ITotalSpec
    {
        IComparable Key { get; }
        string Name { get; }
        SortOrder SortOrder { get; }
    }
}

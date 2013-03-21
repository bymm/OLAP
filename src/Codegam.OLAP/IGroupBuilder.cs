using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public interface IGroupBuilder
    {
        IGroup Build(IOlapDataVector dataVector);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public interface IAggregator
    {
        IAggregator CleanClone();
        object Value { get; }
        void Aggregate(IOlapDataVector dataVector);
    }
}

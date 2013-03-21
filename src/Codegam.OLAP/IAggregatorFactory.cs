using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public interface IAggregatorFactory
    {
        IAggregator CreateCount<T>(Func<IOlapDataVector, bool> aggregatePred = null);
        IAggregator CreateSum<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null);
        IAggregator CreateMin<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null);
        IAggregator CreateMax<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null);
        IAggregator CreateAvg<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null);
    }
}

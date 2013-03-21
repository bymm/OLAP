using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP.Aggregators
{
    public class DefaultAggregatorFactory : IAggregatorFactory
    {
        public IAggregator CreateCount<T>(Func<IOlapDataVector, bool> aggregatePred = null)
        {
            return new CountAggregator<T>(aggregatePred);
        }

        public IAggregator CreateSum<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null)
        {
            return new SumAggregator<T>(valueSelector, aggregatePred);
        }

        public IAggregator CreateMin<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null)
        {
            return new MinAggregator<T>(valueSelector, aggregatePred);
        }

        public IAggregator CreateMax<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null)
        {
            return new MaxAggregator<T>(valueSelector, aggregatePred);
        }

        public IAggregator CreateAvg<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null)
        {
            return new AvgAggregator<T>(valueSelector, aggregatePred);
        }
    }
}

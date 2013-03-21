using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP.Aggregators
{
    public class SumAggregator<T> : Aggregator<T>
    {
        public SumAggregator(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred) : base(valueSelector, aggregatePred) { }

        protected override Aggregator<T> CleanCloneT()
        {
            return new SumAggregator<T>(ValueSelector, AggregatePred);
        }

        protected override void AggregateValue(IOlapDataVector dataVector)
        {
            ValueT = Add(ValueT, GetDataVectorValue<T>(dataVector));
        }
    }
}

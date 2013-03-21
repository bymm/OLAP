using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP.Aggregators
{
    public class CountAggregator<T> : Aggregator<T>
    {
        public CountAggregator(Func<IOlapDataVector, bool> aggregatePred) : base(null, aggregatePred) { }

        protected override Aggregator<T> CleanCloneT()
        {
            return new CountAggregator<T>(AggregatePred);
        }

        protected override void AggregateValue(IOlapDataVector dataVector)
        {
            ValueT = Add(ValueT, Cast<T>(1));
        }
    } 
}

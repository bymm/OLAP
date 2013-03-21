using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Codegam.OLAP.Aggregators
{
    public class AvgAggregator<T> : Aggregator<T> 
    {
        T Sum { get; set; }
        int Count { get; set; }

        public AvgAggregator(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred) : base(valueSelector, aggregatePred) { }

        protected override Aggregator<T> CleanCloneT()
        {
            return new AvgAggregator<T>(ValueSelector, AggregatePred);
        }

        protected override void AggregateValue(IOlapDataVector dataVector)
        {
            Sum = Add(Sum, GetDataVectorValue<T>(dataVector));
            Count++;
            ValueT = Divide(Sum, Cast<T>(Count));
        }
    }
}

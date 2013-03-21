using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP.Aggregators
{
    public class MaxAggregator<T> : Aggregator<T>
    {
        private bool _first;

        public MaxAggregator(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred) : base(valueSelector, aggregatePred) { }

        protected override Aggregator<T> CleanCloneT()
        {
            return new MaxAggregator<T>(ValueSelector, AggregatePred);
        }

        protected override void AggregateValue(IOlapDataVector dataVector)
        {
            if (!_first)
            {
                _first = true;
                ValueT = GetDataVectorValue<T>(dataVector);
            }
            else
                if (Greater(GetDataVectorValue<T>(dataVector), ValueT))
                    ValueT = GetDataVectorValue<T>(dataVector);
        }
    }
}

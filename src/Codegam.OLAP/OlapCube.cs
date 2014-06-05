using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codegam.OLAP.Aggregators;

namespace Codegam.OLAP
{
    public class OlapCube : ITotalSpec
    {
        IAggregatorFactory AggregatorFactory { get; set; }
        List<IGroupBuilder> GroupBuilders { get; set; }
        List<IAggregator> Aggregators { get; set; }
        List<string> GroupTitles { get; set; }
        List<string> ValueTitles { get; set; }
        public ShowTotal ShowTotal { get; set; }
        public string TotalName { get; set; }

        public OlapCube()
            : this(new DefaultAggregatorFactory())
        {
        }

        public OlapCube(IAggregatorFactory aggregatorFactory)
        {
            AggregatorFactory = aggregatorFactory;
            GroupBuilders = new List<IGroupBuilder>();
            Aggregators = new List<IAggregator>();
            GroupTitles = new List<string>();
            ValueTitles = new List<string>();
            ShowTotal = ShowTotal.None;
            TotalName = "";
        }

        public OlapCube GroupBy(IGroupBuilder groupBuilder, string title)
        {
            return ThenBy(groupBuilder, title);
        }

        public OlapCube ThenBy(IGroupBuilder groupBuilder, string title)
        {
            GroupBuilders.Add(groupBuilder);
            GroupTitles.Add(title);
            return this;
        }

        public GroupSpec<T> GroupBy<T>(Func<IOlapDataVector, object> groupSelector, string title = "")
        {
            return ThenBy<T>(groupSelector, title);
        }

        internal GroupSpec<T> ThenBy<T>(Func<IOlapDataVector, object> groupSelector, string title = "")
        {
            var result = new GroupSpec<T>(this, groupSelector);
            GroupBuilders.Add(result);
            GroupTitles.Add(title);
            return result;
        }

        public OlapCube Aggregate(IAggregator aggregator, string title = "")
        {
            Aggregators.Add(aggregator);
            ValueTitles.Add(title);
            return this;
        }

        public OlapCube Count<T>(Func<IOlapDataVector, bool> aggregatePred = null, string title = "Count")
        {
            Aggregators.Add(AggregatorFactory.CreateCount<T>(aggregatePred));
            ValueTitles.Add(title);
            return this;
        }

        public OlapCube Sum<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null, string title = "Sum")
        {
            Aggregators.Add(AggregatorFactory.CreateSum<T>(valueSelector, aggregatePred));
            ValueTitles.Add(title);
            return this;
        }

        public OlapCube Min<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null, string title = "Min")
        {
            Aggregators.Add(AggregatorFactory.CreateMin<T>(valueSelector, aggregatePred));
            ValueTitles.Add(title);
            return this;
        }

        public OlapCube Max<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null, string title = "Max")
        {
            Aggregators.Add(AggregatorFactory.CreateMax<T>(valueSelector, aggregatePred));
            ValueTitles.Add(title);
            return this;
        }

        public OlapCube Avg<T>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null, string title = "Avg")
        {
            Aggregators.Add(AggregatorFactory.CreateAvg<T>(valueSelector, aggregatePred));
            ValueTitles.Add(title);
            return this;
        }

        public OlapCube NameTotalAs(string totalName)
        {
            TotalName = totalName;
            return this;
        }

        public OlapCube ShowTotalAs(ShowTotal showTotal)
        {
            ShowTotal = showTotal;
            return this;
        }

        public OlapCube ShowTotalTop()
        {
            return ShowTotalAs(ShowTotal.Top);
        }

        public OlapCube ShowTotalBottom()
        {
            return ShowTotalAs(ShowTotal.Bottom);
        }
        
        public PivotTable PreparePivotTable(OlapDataSource dataSource)
        {
            var result = new PivotTable(this, Aggregators, SortOrder.Asc, GroupTitles, ValueTitles);            
            foreach (var dataVector in dataSource)
            {
                var groups = ResolveGroups(dataVector).ToList();
                if (groups.Any())
                    result.SortOrder = groups.First().SortOrder;
                result.Process(dataVector, groups);
            }
            return result;
        }

        IEnumerable<IGroup> ResolveGroups(IOlapDataVector dataVector)
        {
            return GroupBuilders.Select(groupBuilder => groupBuilder.Build(dataVector));
        }
    }
}

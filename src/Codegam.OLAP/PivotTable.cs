using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public class PivotTable : PivotBaseGroup
    {
        internal PivotTable(ITotalSpec totalSpec, IEnumerable<IAggregator> aggregators, SortOrder sortOrder, IEnumerable<string> groupTitles, IEnumerable<string> valueTitles)
            : base(totalSpec, aggregators, sortOrder)
        {
            GroupTitles = groupTitles;
            ValueTitles = valueTitles;
        }

        public IEnumerable<string> GroupTitles { get; private set; }

        public IEnumerable<string> ValueTitles { get; private set; }
    }
}

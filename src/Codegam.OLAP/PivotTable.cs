using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public class PivotTable : PivotBaseGroup
    {
        internal PivotTable(ITotalSpec totalSpec, IEnumerable<IAggregator> aggregators, IEnumerable<string> groupTitles, IEnumerable<string> valueTitles)
            : base(totalSpec, aggregators)
        {
            GroupTitles = groupTitles;
            ValueTitles = valueTitles;
        }

        public IEnumerable<PivotTableGroup> Groups
        {
            get { return GroupList; }
        }

        public IEnumerable<string> GroupTitles { get; private set; }

        public IEnumerable<string> ValueTitles { get; private set; }
    }
}

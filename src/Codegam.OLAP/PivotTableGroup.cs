using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public class PivotTableGroup : PivotBaseGroup
    {
        public IComparable Key { get; private set; }
        public string Name { get; private set; }        

        internal PivotTableGroup(IGroup group, IEnumerable<IAggregator> aggregators, SortOrder sortOrder)
            : base(group, aggregators, sortOrder)
        {
            Key = group.Key;
            Name = group.Name;            
        }
    }
}

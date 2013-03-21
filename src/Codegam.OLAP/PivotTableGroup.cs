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

        public PivotTableGroup(IGroup group, IEnumerable<IAggregator> aggregators)
            : base(group, aggregators)
        {
            Key = group.Key;
            Name = group.Name;            
        }

        public IEnumerable<PivotTableGroup> ChildGroups
        {
            get { return GroupList; }
        }
    }
}

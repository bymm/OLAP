using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public abstract class PivotBaseGroup 
    {
        List<IAggregator> Aggregators { get; set; }
        protected List<PivotTableGroup> GroupList { get; private set; }
        Dictionary<IComparable, PivotTableGroup> GroupMap { get; set; }
        PivotTableGroup NullKeyGroup { get; set; }
        public ShowTotal ShowTotal { get; private set; }
        public string TotalName { get; private set; }

        protected PivotBaseGroup(ITotalSpec totalSpec, IEnumerable<IAggregator> aggregators)
        {
            ShowTotal = totalSpec.ShowTotal;
            TotalName = totalSpec.TotalName;
            Aggregators = new List<IAggregator>(aggregators.Select(a => a.CleanClone()));
            GroupList = new List<PivotTableGroup>();
            GroupMap = new Dictionary<IComparable, PivotTableGroup>();            
        }

        public IEnumerable<string> FormatValues
        {
            get { return Aggregators.Select(a => Convert.ToString(a.Value)); }
        }

        public T Value<T>(int index)
        {
            return (T)Convert.ChangeType(Aggregators[index].Value, typeof(T));
        }

        public int Count
        {
            get 
            {
                if (HasChildGroups)
                    return GroupList.Sum(ptg => ptg.Count);
                return 1;
            }
        }

        public bool HasChildGroups
        {
            get { return GroupList.Count > 0; }
        }

        public bool ContainsNoOrSingleChild(bool recursive)
        {
            if (GroupList.Count > 1)
                return false;
            if (recursive)
                return GroupList.All(cg => cg.ContainsNoOrSingleChild(recursive));
            return true;
        }

        internal void Process(IOlapDataVector dataVector, IEnumerable<IGroup> groupList)
        {
            Aggregators.ForEach(a => a.Aggregate(dataVector));
            IGroup group = groupList.FirstOrDefault();
            if (group == null)
                return;
            GetOrCreateGroup(group).Process(dataVector, groupList.Skip(1));
        }

        PivotTableGroup GetOrCreateGroup(IGroup group)
        {
            if (group.Key == null)
            {
                if (NullKeyGroup == null)
                {
                    NullKeyGroup = new PivotTableGroup(group, Aggregators);
                    GroupList.Add(NullKeyGroup);
                }
                return NullKeyGroup;
            }
            PivotTableGroup result;
            if (GroupMap.TryGetValue(group.Key, out result))
                return result;
            result = new PivotTableGroup(group, Aggregators);
            GroupList.Add(result);
            GroupMap[group.Key] = result;
            return result;
        }

        internal class Group : IGroup
        {
            public IComparable Key { get; private set; }
            public string Name { get; private set; }
            public ShowTotal ShowTotal { get; private set; }
            public string TotalName { get; private set; }

            internal Group(IComparable key, string name, ShowTotal showTotalPosition, string totalName)
            {
                Key = key;
                Name = name;
                ShowTotal = showTotalPosition;
                TotalName = totalName;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public class GroupSpec<T> : IGroupBuilder
    {
        OlapCube Cube { get; set; }
        Func<IOlapDataVector, object> GroupSelector { get; set; }
        Func<IOlapDataVector, IComparable> KeySelector { get; set; }
        Func<IOlapDataVector, string> NameSelector { get; set; }
        Func<IOlapDataVector, string> TotalSelector { get; set; }
        ShowTotal ShowTotal { get; set; }

        internal GroupSpec(OlapCube cube, Func<IOlapDataVector, object> groupSelector)
        {
            Cube = cube;
            GroupSelector = groupSelector;
            KeySelector = v => groupSelector(v) as IComparable;
            NameSelector = v => Convert.ToString(groupSelector(v));
            ShowTotal = ShowTotal.Top;
        }

        public GroupSpec<T> KeyGroupAs(Func<T, IComparable> keySelector)
        {
            KeySelector = v => keySelector((T)GroupSelector(v));
            return this;
        }

        public GroupSpec<T> NameGroupAs(Func<T, string> nameSelector)
        {
            NameSelector = v => nameSelector((T)GroupSelector(v));
            return this;
        }

        public GroupSpec<T> NameTotalAs(Func<T, string> totalSelector)
        {
            TotalSelector = v => totalSelector((T)GroupSelector(v));
            return this;
        }

        public GroupSpec<T> ShowTotalAs(ShowTotal showTotal)
        {
            ShowTotal = showTotal;
            return this;
        }

        public GroupSpec<T> ShowTotalTop()
        {
            return ShowTotalAs(ShowTotal.Top);
        }

        public GroupSpec<T> ShowTotalBottom()
        {
            return ShowTotalAs(ShowTotal.Bottom);
        }

        public GroupSpec<V> ThenBy<V>(Func<IOlapDataVector, object> groupSelector, string title = "")
        {
            return Cube.ThenBy<V>(groupSelector, title);
        }

        public OlapCube Count<V>(Func<IOlapDataVector, bool> aggregatePred = null, string title = "Count")
        {
            return Cube.Count<V>(aggregatePred, title);
        }

        public OlapCube Sum<V>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null, string title = "Sum")
        {
            return Cube.Sum<V>(valueSelector, aggregatePred, title);
        }

        public OlapCube Min<V>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null, string title = "Min")
        {
            return Cube.Min<V>(valueSelector, aggregatePred, title);
        }

        public OlapCube Max<V>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null, string title = "Max")
        {
            return Cube.Max<V>(valueSelector, aggregatePred, title);
        }

        public OlapCube Avg<V>(Func<IOlapDataVector, object> valueSelector, Func<IOlapDataVector, bool> aggregatePred = null, string title = "Avg")
        {
            return Cube.Avg<V>(valueSelector, aggregatePred, title);
        }

        IGroup IGroupBuilder.Build(IOlapDataVector dataVector)
        {
            IComparable groupKey = KeySelector(dataVector);
            string groupName = NameSelector(dataVector);
            string totalName = TotalSelector == null ? "" : TotalSelector(dataVector);
            return new PivotBaseGroup.Group(groupKey, groupName, ShowTotal, totalName);
        }
    }
}

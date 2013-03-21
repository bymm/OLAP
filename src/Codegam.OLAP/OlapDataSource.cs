using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codegam.OLAP
{
    public class OlapDataSource : IOlapDataSource
    {
        List<OlapDataVector> Data { get; set; }

        public OlapDataSource()
        {
            Data = new List<OlapDataVector>();
        }

        public void Add(params object[] dataRow)
        {
            Data.Add(new OlapDataVector(dataRow));
        }

        public int DataCount
        {
            get { return Data.Count(); }
        }

        public IEnumerator<IOlapDataVector> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        internal class OlapDataVector : IOlapDataVector
        {
            object[] Data { get; set; }

            internal OlapDataVector(object[] dataRow)
            {
                Data = new object[dataRow.Length];
                for(int i = 0; i < dataRow.Length; i++)
                    Data[i] = dataRow[i];
            }

            object IOlapDataVector.this[int index]
            {
                get { return Data[index]; }
            }

            T IOlapDataVector.Get<T>(int index)
            {
                return (T)Data[index];
            }
        }
    }
}

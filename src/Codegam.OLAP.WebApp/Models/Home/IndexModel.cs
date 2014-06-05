using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codegam.OLAP.WebApp.Models.Home
{
    public class IndexModel
    {
        public PivotTable UserBankTable
        {
            get
            {
                OlapCube cube = new OlapCube()
                                        .GroupBy<UserType>(v => v[0], "User type")
                                            .NameGroupAs(ut => ut == UserType.Private ? "Private users" : "Corporate users")
                                        .ThenBy<string>(v => v[1], "Bank")
                                            .KeyGroupAs(b => (b ?? "").ToLower())
                                            .NameGroupAs(b => b.ToUpper())
                                        .Min<int>(v => v[3], title: "Min")
                                        .Max<int>(v => v[3], title: "Max")
                                        .Count<int>(v => v[3] != null, "Count")
                                        .Sum<int>(v => v[3], title: "Sum")
                                        .Avg<double>(v => v[3], title: "Avg");
                //.FormatAs(v => string.Format("{0:F2}", v));                                   
                PivotTable pivotTable = cube.PreparePivotTable(PrepareDataSource());
                return pivotTable;
            }
        }

        private OlapDataSource PrepareDataSource()
        {
            var dataSource = new OlapDataSource();
            dataSource.Add(UserType.Private, "BPU", new DateTime(2013, 9, 12), 4);
            dataSource.Add(UserType.Private, "bpb", new DateTime(2013, 9, 2), 2);
            dataSource.Add(UserType.Corporate, "BPB", new DateTime(2013, 9, 4), 6);
            dataSource.Add(UserType.Private, "BPB", new DateTime(2013, 8, 1), 1);
            dataSource.Add(UserType.Corporate, "BPU", new DateTime(2013, 8, 24), 2);
            dataSource.Add(UserType.Corporate, "BNP", new DateTime(2013, 8, 29), 3);
            return dataSource;
        }

        public PivotTable DateBankTable
        {
            get
            {
                OlapCube cube = new OlapCube()
                                    .GroupBy<DateTime>(v => v[2])
                                        .KeyGroupAs(rd => rd.Year * 12 + rd.Month)
                                        .NameGroupAs(rd => rd.ToString("MMM yyyy"))                                        
                                        .NameTotalAs(rd => "Total in " + rd.ToString("MMM yyyy"))
                                        .ShowTotalBottom()
                                    .ThenBy<string>(v => v[1], "Bank")
                                            .KeyGroupAs(b => (b ?? "").ToLower())
                                            .NameGroupAs(b => b.ToUpper())
                                        .Min<int>(v => v[3], title: "Min")
                                        .Max<int>(v => v[3], title: "Max")
                                        .Count<int>(title: "Count")
                                        .Sum<int>(v => v[3], title: "Sum")
                                        .Avg<double>(v => v[3], title: "Avg")
                                    .ShowTotalBottom()
                                    .NameTotalAs("Total");
                //.FormatAs(v => string.Format("{0:F2}", v));                                   
                PivotTable pivotTable = cube.PreparePivotTable(PrepareDataSource());
                return pivotTable;
            }
        }

        public PivotTable DateBankUserTypeTable
        {
            get
            {
                OlapCube cube = new OlapCube()
                                    .GroupBy<DateTime>(v => v[2])
                                        .KeyGroupAs(rd => rd.Year * 12 + rd.Month)
                                        .NameGroupAs(rd => rd.ToString("MMM yyyy"))
                                        .NameTotalAs(rd => "Total in " + rd.ToString("MMM yyyy"))
                                        .ShowTotalBottom()
                                        .OrderByDescending()
                                    .ThenBy<UserType>(v => v[0], "User type")
                                        .NameGroupAs(ut => ut == UserType.Private ? "Private users" : "Corporate users")
                                        .ShowTotalBottom()
                                    .ThenBy<string>(v => v[1], "Bank")
                                        .KeyGroupAs(b => (b ?? "").ToLower())
                                        .NameGroupAs(b => b.ToUpper())
                                        .ShowTotalBottom()
                                    .Min<int>(v => v[3], title: "Min")
                                    .Max<int>(v => v[3], title: "Max")
                                    .Count<int>(title: "Count")
                                    .Sum<int>(v => v[3], title: "Sum")
                                    .Avg<double>(v => v[3], title: "Avg")
                                    .ShowTotalBottom()
                                    .NameTotalAs("Total");
                //.FormatAs(v => string.Format("{0:F2}", v));                                   
                PivotTable pivotTable = cube.PreparePivotTable(PrepareDataSource());
                return pivotTable;
            }
        }
    }
}
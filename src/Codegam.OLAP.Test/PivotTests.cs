using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codegam.OLAP;
using FluentAssertions;

namespace Codegam.OLAP.Test
{
    enum UserType { Private, Corporate };

    [TestClass]
    public class PivotTests
    {
        [TestMethod]
        public void GroupByUserTypeAndBank()
        {
            OlapDataSource dataSource = CreateTestSource();
            dataSource.DataCount.Should().Be(6);
            OlapCube cube = new OlapCube()
                                    .GroupBy<UserType>(v => v[0])
                                        .NameGroupAs(ut => ut == UserType.Private ? "Private users" : "Corporate users")
                                    .ThenBy<string>(v => v[1])
                                        .KeyGroupAs(b => (b ?? "").ToLower())
                                    .Sum<int>(v => v[3])
                                    .Min<int>(v => v[3])
                                    .Max<int>(v => v[3])
                                    .Avg<double>(v => v[3])
                                    .Count<int>();
            PivotTable table = cube.PreparePivotTable(dataSource);
            table.Value<int>(0).Should().Be(18); //sum
            table.Value<int>(1).Should().Be(1); //min
            table.Value<int>(2).Should().Be(6); //max
            table.Value<double>(3).Should().BeInRange(3 - 0.01, 3 + 0.01); //avg
            table.Value<int>(4).Should().Be(6); // count
            table.Groups.Should().HaveCount(2);
            
            var privateUsersGroup = table.Groups.First();
            privateUsersGroup.Key.Should().Be(UserType.Private);
            privateUsersGroup.Name.Should().Be("Private users");
            privateUsersGroup.Value<int>(0).Should().Be(7);
            privateUsersGroup.Value<int>(1).Should().Be(1);
            privateUsersGroup.Value<int>(2).Should().Be(4);
            privateUsersGroup.Value<double>(3).Should().BeInRange(7.0 / 3 - 0.01, 7.0 / 3 + 0.01);
            privateUsersGroup.Value<int>(4).Should().Be(3);
            
            privateUsersGroup.ChildGroups.Should().HaveCount(2);

            privateUsersGroup.ChildGroups.First().Key.Should().Be("bpb");
            privateUsersGroup.ChildGroups.First().Name.Should().Be("BPB");
            privateUsersGroup.ChildGroups.First().Value<int>(0).Should().Be(3);
            privateUsersGroup.ChildGroups.First().Value<int>(1).Should().Be(1);
            privateUsersGroup.ChildGroups.First().Value<int>(2).Should().Be(2);
            privateUsersGroup.ChildGroups.First().Value<double>(3).Should().BeInRange(3.0 / 2 - 0.01, 3.0 / 2 + 0.01);
            privateUsersGroup.ChildGroups.First().Value<int>(4).Should().Be(2);

            privateUsersGroup.ChildGroups.Last().Key.Should().Be("bpu");
            privateUsersGroup.ChildGroups.Last().Name.Should().Be("BPU");
            privateUsersGroup.ChildGroups.Last().Value<int>(0).Should().Be(4);
            privateUsersGroup.ChildGroups.Last().Value<int>(1).Should().Be(4);
            privateUsersGroup.ChildGroups.Last().Value<int>(2).Should().Be(4);
            privateUsersGroup.ChildGroups.Last().Value<double>(3).Should().BeInRange(4 - 0.01, 4 + 0.01);
            privateUsersGroup.ChildGroups.Last().Value<int>(4).Should().Be(1);

            var corporateUsersGroup = table.Groups.Last();
            corporateUsersGroup.Key.Should().Be(UserType.Corporate);
            corporateUsersGroup.Name.Should().Be("Corporate users");
            corporateUsersGroup.Value<int>(0).Should().Be(11);
            corporateUsersGroup.Value<int>(1).Should().Be(2);
            corporateUsersGroup.Value<int>(2).Should().Be(6);
            corporateUsersGroup.Value<double>(3).Should().BeInRange(11.0 / 3 - 0.01, 11.0 / 3 + 0.01);
            corporateUsersGroup.Value<int>(4).Should().Be(3);

            corporateUsersGroup.ChildGroups.Should().HaveCount(3);

            corporateUsersGroup.ChildGroups.First().Key.Should().Be("bpb");
            corporateUsersGroup.ChildGroups.First().Name.Should().Be("BPB");
            corporateUsersGroup.ChildGroups.First().Value<int>(0).Should().Be(6);
            corporateUsersGroup.ChildGroups.First().Value<int>(1).Should().Be(6);
            corporateUsersGroup.ChildGroups.First().Value<int>(2).Should().Be(6);
            corporateUsersGroup.ChildGroups.First().Value<double>(3).Should().BeInRange(6 - 0.01, 6 + 0.01);
            corporateUsersGroup.ChildGroups.First().Value<int>(4).Should().Be(1);

            corporateUsersGroup.ChildGroups.Last().Key.Should().Be("bnp");
            corporateUsersGroup.ChildGroups.Last().Name.Should().Be("BNP");
            corporateUsersGroup.ChildGroups.Last().Value<int>(0).Should().Be(3);
            corporateUsersGroup.ChildGroups.Last().Value<int>(1).Should().Be(3);
            corporateUsersGroup.ChildGroups.Last().Value<int>(2).Should().Be(3);
            corporateUsersGroup.ChildGroups.Last().Value<double>(3).Should().BeInRange(3 - 0.01, 3 + 0.01);
            corporateUsersGroup.ChildGroups.Last().Value<int>(4).Should().Be(1);
        }

        private static OlapDataSource CreateTestSource()
        {
            OlapDataSource dataSource = new OlapDataSource();
            dataSource.Add(UserType.Private, "BPB", new DateTime(2013, 8, 1), 1);
            dataSource.Add(UserType.Private, "bpb", new DateTime(2013, 9, 12), 2);
            dataSource.Add(UserType.Private, "BPU", new DateTime(2013, 9, 12), 4);
            dataSource.Add(UserType.Corporate, "BPB", new DateTime(2013, 9, 4), 6);
            dataSource.Add(UserType.Corporate, "BPU", new DateTime(2013, 8, 1), 2);
            dataSource.Add(UserType.Corporate, "BNP", new DateTime(2013, 8, 1), 3);
            return dataSource;
        }

        [TestMethod]
        public void GroupByBankWithNull()
        {
            OlapDataSource dataSource = new OlapDataSource();
            dataSource.Add(UserType.Private, "bpb", new DateTime(2013, 8, 1), 1);
            dataSource.Add(UserType.Private, null, new DateTime(2013, 9, 2), 2);
            OlapCube cube = new OlapCube()
                                    .GroupBy<string>(v => v[1])
                                        .NameGroupAs(b => (b ?? "").ToUpper())
                                    .Sum<int>(v => v[3]);
            PivotTable table = cube.PreparePivotTable(dataSource);
            table.Value<int>(0).Should().Be(3);
            table.Groups.Should().HaveCount(2);
            var group = table.Groups.First();
            group.Key.Should().Be("bpb");
            group.Name.Should().Be("BPB");
            group = table.Groups.Last();
            group.Key.Should().BeNull();
            group.Name.Should().Be("");
        }

        [TestMethod]
        public void GroupByDateTime_MonthAndDate()
        {
            OlapDataSource dataSource = CreateTestSource();
            OlapCube cube = new OlapCube()
                                    .GroupBy<DateTime>(v => v[2])
                                        .KeyGroupAs(rd => rd.Year * 12 + rd.Month)
                                        .NameGroupAs(rd => rd.ToString("MMM yyyy"))
                                    .ThenBy<DateTime>(v => v[2])
                                        .KeyGroupAs(rd => rd.Date)
                                        .NameGroupAs(rd => rd.ToString("dd/MM/yyyy"))
                                    .Sum<int>(v => v[3]);
            PivotTable table = cube.PreparePivotTable(dataSource);
            table.Value<int>(0).Should().Be(18);
            table.Groups.Should().HaveCount(2);
            var monthGroup = table.Groups.First();
            monthGroup.Key.Should().Be(2013 * 12 + 8);
            monthGroup.Name.Should().Be("Aug 2013");
            monthGroup.Value<int>(0).Should().Be(6);
            monthGroup.ChildGroups.Should().HaveCount(1);
            var dateGroup = monthGroup.ChildGroups.First();
            dateGroup.Key.Should().Be(new DateTime(2013, 8, 1));
            dateGroup.Name.Should().Be("01/08/2013");
            dateGroup.Value<int>(0).Should().Be(6);

            monthGroup = table.Groups.Last();
            monthGroup.Key.Should().Be(2013 * 12 + 9);
            monthGroup.Name.Should().Be("Sep 2013");
            monthGroup.Value<int>(0).Should().Be(12);
            monthGroup.ChildGroups.Should().HaveCount(2);
            dateGroup = monthGroup.ChildGroups.First();
            dateGroup.Key.Should().Be(new DateTime(2013, 9, 12));
            dateGroup.Name.Should().Be("12/09/2013");
            dateGroup.Value<int>(0).Should().Be(6);
        }

        [TestMethod]
        public void GroupByDateTime()
        {
            OlapDataSource dataSource = CreateTestSource();
            OlapCube cube = new OlapCube()
                                    .GroupBy<DateTime>(v => v[2])
                                    .Sum<int>(v => v[3]);
            PivotTable table = cube.PreparePivotTable(dataSource);
            table.Value<int>(0).Should().Be(18);
        }

        [TestMethod]
        public void GroupWithPredicate()
        {
            OlapDataSource dataSource = CreateTestSource();
            OlapCube cube = new OlapCube()
                                    .GroupBy<UserType>(v => v[0])
                                    .ThenBy<string>(v => v[1])
                                    .Sum<int>(v => v[3], v => string.Equals(v.Get<string>(1), "bpb", StringComparison.InvariantCultureIgnoreCase))
                                    .Avg<double>(v => v[3], v => string.Equals(v.Get<string>(1), "bpb", StringComparison.InvariantCultureIgnoreCase))
                                    .Count<int>(v => string.Equals(v.Get<string>(1), "bpb", StringComparison.InvariantCultureIgnoreCase));
            PivotTable table = cube.PreparePivotTable(dataSource);
            table.Value<int>(0).Should().Be(9); //sum
            table.Value<double>(1).Should().BeInRange(3 - 0.01, 3 + 0.01); //avg
            table.Value<int>(2).Should().Be(3); // count
            table.Groups.Should().HaveCount(2);
        }
    }
}

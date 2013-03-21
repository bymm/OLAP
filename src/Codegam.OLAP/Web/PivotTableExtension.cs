using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Codegam.OLAP.Web
{
    public static class PivotTableExtension
    {
        public static string RenderHtmlTable(this PivotTable pivotTable, string title = "")
        {
            TagBuilder builder = new TagBuilder("div");
            builder.AddCssClass("olap-pivot-table");
            builder.InnerHtml = BuildTitle(title) + BuildTable(pivotTable);
            return builder.ToString();
        }

        private static string BuildTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return "";
            return string.Format(@"<div class=""title"">{0}</div>", title);
        }

        private static string BuildTable(PivotTable pivotTable)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"<table border=""1"">");
            BuildHeader(pivotTable, builder);
            BuildBody(pivotTable, builder);            
            builder.Append("</table>");
            return builder.ToString();
        }

        private static void BuildHeader(PivotTable pivotTable, StringBuilder builder)
        {
            builder.Append("<thead><tr>");
            foreach (string groupTitle in pivotTable.GroupTitles)
                builder.AppendFormat(@"<th class=""group-title"">{0}</th>", groupTitle);
            foreach (string valueTitle in pivotTable.ValueTitles)
                builder.AppendFormat(@"<th class=""value-title"">{0}</th>", valueTitle);
            builder.Append("</tr></thead>");
        }

        private static void BuildBody(PivotTable pivotTable, StringBuilder builder)
        {
            builder.Append("<tbody>");
            if (pivotTable.ShowTotal == ShowTotal.Top)
                BuildTotal(pivotTable, builder);
            Stack<bool> startRow = new Stack<bool>();
            BuildGroups(pivotTable.Groups, 0, pivotTable.GroupTitles.Count(), startRow, builder);
            if (pivotTable.ShowTotal == ShowTotal.Bottom)
                BuildTotal(pivotTable, builder);
            builder.Append("</tbody>");
        }

        private static void BuildGroups(IEnumerable<PivotTableGroup> groups, int level, int levels, Stack<bool> startRow, StringBuilder builder)
        {   
            foreach (PivotTableGroup group in groups)
                BuildGroup(group, level, levels, startRow, builder);
        }

        private static void BuildGroup(PivotTableGroup group, int level, int levels, Stack<bool> startRow, StringBuilder builder)
        {
            if (startRow.Count == 0)
            {
                builder.AppendFormat("<tr>");
                startRow.Push(true);
            }

            int rowSpan = group.Count + (group.HasChildGroups && group.ShowTotal == ShowTotal.Top ? 1 : 0);
            builder.AppendFormat(@"<td{1} class=""group"">{0}</td>", group.Name, rowSpan > 1 ? string.Format(@" rowSpan=""{0}""", rowSpan) : "");

            if (group.ShowTotal == ShowTotal.Top)
            {
                BuildPrefixCells(level, levels, builder);
                BuildValues(group.FormatValues, builder);
                builder.Append("</tr>");
                startRow.Pop();
            }

            BuildGroups(group.ChildGroups, level + 1, levels, startRow, builder);

            if (group.ShowTotal == ShowTotal.Bottom && !group.HasChildGroups)
            {
                BuildValues(group.FormatValues, builder);
                builder.Append("</tr>");
                startRow.Pop();
            }

            if (group.ShowTotal == ShowTotal.Bottom && !group.ContainsNoOrSingleChild(true))
            {
                builder.Append(@"<tr class=""total"">");
                builder.AppendFormat(@"<td class=""group"">{0}</td>", group.TotalName);
                BuildPrefixCells(level, levels, builder);
                BuildValues(group.FormatValues, builder);                
                builder.Append("</tr>");
            }
        }

        private static void BuildPrefixCells(int level, int levels, StringBuilder builder)
        {
            for (int i = level + 1; i < levels; i++)
                builder.AppendFormat("<td></td>");
        }

        private static void BuildValues(IEnumerable<string> formatValues, StringBuilder builder)
        {
            foreach (string formatValue in formatValues)
                builder.AppendFormat(@"<td class=""value"">{0}</td>", formatValue);
        }

        private static void BuildTotal(PivotTable pivotTable, StringBuilder builder)
        {
            builder.Append(@"<tr class=""total"">");
            builder.AppendFormat(@"<td class=""group"">{0}</td>", pivotTable.TotalName);
            BuildPrefixCells(0, pivotTable.GroupTitles.Count(), builder);
            BuildValues(pivotTable.FormatValues, builder);
            builder.Append("</tr>");
        }
    }
}

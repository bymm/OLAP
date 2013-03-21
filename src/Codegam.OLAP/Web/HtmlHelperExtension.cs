using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Codegam.OLAP.Web
{
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString PivotTable(this HtmlHelper html, PivotTable pivotTable, string title = "")
        {
            return new MvcHtmlString(pivotTable.RenderHtmlTable(title));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Codegam.OLAP.WebApp.Models;
using Codegam.OLAP.WebApp.Models.Home;

namespace Codegam.OLAP.WebApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {   
            return View(new IndexModel());
        }

    }
}

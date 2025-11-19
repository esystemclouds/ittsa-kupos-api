using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IttsabusAPI.EndPoint.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "EndPoint - Kupos Ittsabus";

            return View();
        }
    }
}

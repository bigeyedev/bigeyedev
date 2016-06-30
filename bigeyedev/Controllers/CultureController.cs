using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bigeyedev.Controllers
{
    public class CultureController : Controller
    {
        // GET: Culture
        public ActionResult Index()
        {
            Session["Culture"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Thai()
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Session["Culture"] = "th-TH";
            return RedirectToAction("Index", "Home");
        }




    }
}
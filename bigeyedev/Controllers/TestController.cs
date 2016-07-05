using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bigeyedev.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult JQ_Dialog()
        {
            return View("JQ_Dialog");
        }

        public ActionResult JQ_Dialog_Dailog_Alt()
        {
            return View("JQ_Dialog_Dailog_Alt");
        }
    }
}
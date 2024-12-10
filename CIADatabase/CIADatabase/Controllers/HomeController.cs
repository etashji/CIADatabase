using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIADatabase.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult CIAIndex()
        {
            return View();
        }
        public ActionResult JediIndex()
        {
            return View();
        }
    }
}
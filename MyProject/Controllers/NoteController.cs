using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers
{
    public class NoteController : Controller
    {
        public ActionResult MCN()
        {
            var value = RouteData.Values["id"];
            if (value == null)
                ViewBag.index = -1;
            else
                ViewBag.index = value;
            return View();
        }
    }
}
using MyProject.DAL.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers
{
    public class SystemController : Controller
    {
        // GET: System
        public ActionResult Menu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMenuList()
        {
            using (var db = new MyProjectEF())
            {
                var queryable = db.uFunctions.Join(db.uCodes,
                          function => function.FunTypeId,
                          code => code.CodeId,
                          (f, c) => new
                          {
                              f.FunId,
                              f.FunName,
                              f.FunLink,
                              f.FunDesc,
                              f.FunParentId,
                              FunParent = f.FunParentId != null ? db.uFunctions.FirstOrDefault(p => p.FunId == f.FunParentId).FunName : string.Empty,
                              f.FunTypeId,
                              FunType = c.Code,
                              f.FunSeq,
                              f.LCV
                          });

                List<object> list = new List<object>();
                foreach (var q in queryable)
                {
                    list.Add(q);
                }
                return Json(new { status = 0, data = list });
            }

        }

        [HttpPost]
        public ActionResult SaveMenu(string json)
        {
            var menu = JsonConvert.DeserializeObject<uFunction>(json);
            
            return Json(new { status = 0, data = "success" });


        }


    }
}
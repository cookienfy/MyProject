using MyProject.DAL;
using MyProject.DAL.Models;
using MyProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetProjects(string name)
        {
            using (ProjectRepository rep = new ProjectRepository())
            {
                IDictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("ProjectName", name);

                var list = rep.Query(dic).ToList();

                return Json(new { data = list.ToList() });
            }
        }

        [HttpPost]
        public ActionResult GetCode(string value)
        {
            CodeRepository codeRepository = new CodeRepository();
            var dic = new Dictionary<string, object>();
            dic["CodeGroup"] = value;
            var list = codeRepository.Query(dic).ToList();

            return Json(new { data = list.ToList() });
        }


        public ActionResult ProjectDetail(string pd)
        {

            var id = Base64Helper.Base64Decode(pd);

            using (ProjectRepository rep = new ProjectRepository())
            {
                IDictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("ProjectId", Convert.ToInt32(id));

                var list = rep.Query(dic).ToList();

                
                return View(list[0]);
            }


        }

    }
}
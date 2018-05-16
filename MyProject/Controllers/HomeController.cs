using MyProject.DAL;
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
            //using (ProjectRepository rep = new ProjectRepository())
            //{
            //    IDictionary<string, object> dic = new Dictionary<string, object>();

            //    dic.Add("ProjectName", name);

            //    var list = rep.Query(dic).ToList();

            //    return Json(new { data = list.ToList() });
            //}
            return null;
        }

        [HttpPost]
        public ActionResult GetCode(string value)
        {
            using (MyProject.DAL.EF.MyProjectEF db = new DAL.EF.MyProjectEF())
            {
                var codeGroup = db.uCodeGroups.FirstOrDefault(p => p.CodeGroup.Equals(value));

                IList<object> datas = new List<object>();
                foreach (var g in codeGroup.uCodes)
                    datas.Add(new
                    {
                        CodeId = g.CodeId,
                        Code = g.Code
                    });

                return Json(new { data = datas });

            }



        }


        public ActionResult ProjectDetail(string pd)
        {

            //var id = Base64Helper.Base64Decode(pd);

            //using (ProjectRepository rep = new ProjectRepository())
            //{
            //    IDictionary<string, object> dic = new Dictionary<string, object>();

            //    dic.Add("ProjectId", Convert.ToInt32(id));

            //    var list = rep.Query(dic).ToList();


            //    return View(list[0]);
            //}
            return View();

        }


        public ActionResult InfoCenter()
        {
            return View();
        }



    }
}
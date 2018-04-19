using MyProject.DAL;
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
        public ActionResult GetParentMenus()
        {
            using (UnitOfWork work = new UnitOfWork())
            {
                var query = work.FunctionRepository.DbSet.Where(p => p.FunTypeId == work.CodeRepository.DbSet.FirstOrDefault(r => r.Code == "Menu").CodeId);
                IList<object> datas = new List<object>();
                foreach (var q in query)
                    datas.Add(new { FunId = q.FunId, FunName = q.FunName });

                return Json(new { data = datas });
            }
        }

        [HttpPost]
        public ActionResult GetMenuList()
        {
            using (UnitOfWork work = new UnitOfWork())
            {
                var queryable = work.FunctionRepository.DbSet.Join(work.CodeRepository.DbSet, f => f.FunTypeId, c => c.CodeId, (f, c) => new
                {
                    f.FunId,
                    f.FunName,
                    f.FunLink,
                    f.FunDesc,
                    f.FunPic,
                    f.FunParentId,
                    FunParent = f.FunParentId != null ? work.FunctionRepository.DbSet.FirstOrDefault(p => p.FunId == f.FunParentId).FunName : string.Empty,
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
        public ActionResult GetMenu(int id)
        {
            using (UnitOfWork work = new UnitOfWork())
            {
                var menu = work.FunctionRepository.DbSet.FirstOrDefault(p => p.FunId == id);
                return Json(menu);
            }
        }

        [HttpPost]
        public ActionResult AddMenu(string value)
        {
            var jsonSetting = new JsonSerializerSettings();
            jsonSetting.NullValueHandling = NullValueHandling.Ignore;

            var menu = JsonConvert.DeserializeObject<uFunction>(value, jsonSetting);
            menu.CreationDate = DateTime.Now;
            using (UnitOfWork work = new UnitOfWork())
            {
                work.FunctionRepository.Add(menu);
                work.SaveChanges();
            }

            return Json(new { value = 0 });
        }

        [HttpPost]
        public ActionResult UpdateMenu(string value)
        {
            var jsonSetting = new JsonSerializerSettings();
            jsonSetting.NullValueHandling = NullValueHandling.Ignore;

            var menu = JsonConvert.DeserializeObject<uFunction>(value, jsonSetting);

            using (UnitOfWork work = new UnitOfWork())
            {
                work.FunctionRepository.Update(menu);
                work.SaveChanges();
            }
            return Json(new { value = 0 });
        }


        public ActionResult User()
        {
            return PartialView("User");
        }

        public ActionResult Account()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetUsers()
        {
            using (UnitOfWork work = new UnitOfWork())
            {
                var users = work.UserRepository.Query(null, null);
                return Json(new { status = 0, data = users });
            }
        }


    }
}
using MyProject.BLL;
using MyProject.DAL;
using MyProject.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers
{

    /// <summary>
    /// value返回类型
    /// 0. 保存成功
    /// 1. 逻辑性错误提示
    /// 8. 记录超过规定的数量
    /// 9. 程序内部错误
    /// value = 1, msg = "" ,data
    /// </summary>

    public class LibraryController : Controller
    {

        private const int iMaxRows = 2000;

        // GET: Library
        public ActionResult Index()
        {
            ViewBag.MaxRows = iMaxRows;

            return View();
        }

        [HttpPost]
        public ActionResult GetLibraries(string doubt)
        {
            IList<object> datas = new List<object>();
            if (string.IsNullOrEmpty(doubt))
                return Json(new { data = datas });

            using (UnitOfWork work = new UnitOfWork())
            {
                var libraries = work.LibraryRepository.Query(p => (p.Doubt.Contains(doubt.Trim()) || p.Fun.Contains(doubt.Trim())) && !p.LCV);
                int iRowCount = libraries.Count();
                if (iRowCount > iMaxRows)
                {
                    var takeMaxRows = libraries.Take(iMaxRows);
                    foreach (var q in takeMaxRows)
                    {
                        datas.Add(new
                        {
                            LibraryId = q.LibraryId,
                            Doubt = q.Doubt,
                            Fun = q.Fun,
                            Contributor = q.Contributor,
                            ContributeDate = q.ContributeDate
                        });
                    }
                    return Json(new { value = 8, msg = string.Format("共查找到{0}条记录, 页面最多显示{1}条记录", iRowCount, iMaxRows), data = datas });
                }
                else
                {
                    foreach (var q in libraries)
                        datas.Add(new
                        {
                            LibraryId = q.LibraryId,
                            Doubt = q.Doubt,
                            Fun = q.Fun,
                            Contributor = q.Contributor,
                            ContributeDate = q.ContributeDate
                        });
                    return Json(new { value = 0, msg = "", data = datas });
                }
            }

        }

        [HttpPost]
        public ActionResult AddLibrary(DAL.EF.uLibrary library)
        {
            DAL.EF.uLibrary lb = null;
            using (UnitOfWork work = new UnitOfWork())
            {
                if (library.LibraryId > 0)
                {
                    work.LibraryRepository.Update(library);
                    lb = library;
                }
                else
                    lb = work.LibraryRepository.Add(library);
                work.SaveChanges();
                return Json(new { value = lb });
            }

        }

        [HttpPost]
        public ActionResult GetLibraryDetail(int id)
        {
            using (UnitOfWork work = new UnitOfWork())
            {
                string imgHtml = "<img src='{0}' class='kv-preview-data file-preview-image' width='200px' height='180px'>";
                var queries = work.LibraryRepository.Query(p => p.LibraryId.Equals(id)).ToList();
                var contexts = work.ContextRepository.Query(p => p.LibraryId.Equals(id));
                IList<string> lsPath = new List<string>();
                IList<object> lsObj = new List<object>();
                if (contexts.Count() > 0)
                {
                    string folder = Request.MapPath("UploadFolder".GetWebKeyValue());
        
                    folder = folder.Replace("\\Library", "");
                    if (!System.IO.Directory.Exists(folder))
                        System.IO.Directory.CreateDirectory(folder);

                    foreach (uContext c in contexts)
                    {
                        string fullname = folder + "\\" + c.FileName;

                        if (!System.IO.File.Exists(fullname))
                            System.IO.File.WriteAllBytes(fullname, c.Content);
                        System.IO.FileInfo info = new System.IO.FileInfo(fullname);

                        lsPath.Add(string.Format(imgHtml, string.Format("UploadFiles/{0}", c.FileName)));
                        lsObj.Add(new
                        {
                            caption = c.FileName,
                            size = info.Length,
                            width = "300px",
                            url = "/Library/DeleteImage",
                            key = c.ContextId
                        });
                    }
                }

                return Json(new { data = queries[0], paths = lsPath, initialPreviewConfig = lsObj.ToArray() });
            }
        }

        [HttpPost]
        public ActionResult DeleteImage(int key)
        {
            string sError = string.Empty;
            try
            {
                using (UnitOfWork work = new UnitOfWork())
                {
                    var contexts = work.ContextRepository.Query(p => p.ContextId.Equals(key));
                    if (contexts.Count() > 0)
                    {
                        work.ContextRepository.Delete(contexts.ToList()[0]);
                        work.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                sError = ex.Message;
            }
            if (sError != "")
            {
                var obj = new
                {
                    status = 1,
                    statusCode = "error",
                    errorMsg = sError
                };
                return Json(new
                {
                    data = obj
                });

            }
            else
            {
                var obj = new
                {
                    status = 0,
                    statusCode = "success",
                    errorMsg = sError
                };
                return Json(new
                {
                    data = obj
                });

            }
        }

        [HttpPost]
        public ActionResult DeleteLibrary(int id)
        {
            using (UnitOfWork work = new UnitOfWork())
            {
                try
                {
                    var model = work.LibraryRepository.Query(p => p.LibraryId.Equals(id));
                    if (model.Count() == 0)
                    {
                        return Json(new { value = 1, msg = "该记录已被删除!" });
                    }
                    else
                    {
                        work.LibraryRepository.Delete(model.ToList()[0]);
                        var contexts = work.ContextRepository.Query(p => p.LibraryId.Equals(id));
                        if (contexts.Count() > 0)
                            foreach (uContext c in contexts)
                                work.ContextRepository.Delete(c);

                        work.SaveChanges();
                        return Json(new { value = 0, msg = "" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { value = 9, msg = ex.Message });
                }
            }
        }

        [HttpPost]
        public string UploadLibraryFiles(string LibraryId)
        {
            int iLibraryId = int.Parse(LibraryId);
            string sError = string.Empty;
            var files = HttpContext.Request.Files;
            try
            {
                string path = Request.MapPath("UploadFolder".GetWebKeyValue());
                path = path.Replace("\\Library", "");
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                using (UnitOfWork work = new UnitOfWork())
                {
                    IList<uContext> lsContext = new List<uContext>();
                    uContext con;
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        con = new uContext();
                        con.FileName = string.Format("{0}_{1}", Guid.NewGuid().ToString(), files[i].FileName);
                        con.Extension = files[i].FileName.Substring(files[i].FileName.LastIndexOf('.') + 1);
                        con.LibraryId = iLibraryId;
                        var stream = files[i].InputStream;
                        stream.Position = 0;
                        byte[] buffers = new byte[stream.Length];
                        stream.Read(buffers, 0, buffers.Length);
                        con.Content = buffers;

                        lsContext.Add(con);

                    }

                    work.ContextRepository.Add(lsContext);

                    work.SaveChanges();

                    foreach (var c in lsContext)
                    {
                        System.IO.File.WriteAllBytes(string.Format(@"{0}\{1}", path, c.FileName), c.Content);
                    }

                }
            }
            catch (Exception ex)
            {
                sError = ex.Message;
            }

            if (sError != "")
            {
                var obj = new
                {
                    status = 1,
                    statusCode = "error",
                    errorMsg = sError
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            else
            {
                var obj = new
                {
                    status = 0,
                    statusCode = "success",
                    errorMsg = sError
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }


        }




        /// <summary>
        /// 获取访问客户端的IPV4地址
        /// </summary>
        /// <returns></returns>
        public string GetClientIPv4Address()
        {
            string ipv4 = String.Empty;
            foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostAddresses(GetClientIP()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            if (ipv4 != String.Empty)
            {
                return ipv4;
            }
            // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，
            // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
            foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostEntry(GetClientIP()).AddressList)
            //foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            return ipv4;
        }
        public string GetClientIP()
        {
            if (null == HttpContext.Request.ServerVariables["HTTP_VIA"])
            {
                return HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                return HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

        }


    }
}
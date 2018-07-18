using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using MyProject.Helpers;
using MyProject.DAL;
using MyProject.DAL.EF;
using System.Linq;
using System.Data.Entity;

namespace MyProject.Tests
{
    [TestClass]
    public class UnitTest1
    {

        #region Dapper

        //string C_ConnectionStringName = "SqlCon";
        //private IDbConnection GetConnection()
        //{
        //    var str = System.Configuration.ConfigurationManager.ConnectionStrings[C_ConnectionStringName].ConnectionString;
        //    if (string.IsNullOrEmpty(str))
        //        throw new ArgumentNullException("Connection string can not null or empty.");
        //    IDbConnection con = new MySqlConnection(str);

        //    return con;
        //}

        //[TestMethod]
        //public void AddCode()
        //{
        //    CodeModel code = new CodeModel();
        //    code.Code = "Low";
        //    code.CodeGroup = "Priority";


        //    IDbConnection db = GetConnection();

        //    string sql = @"insert into ucode
        //                  (
        //                    codeId
        //                    ,code
        //                    ,CodeGroup
        //                   )
        //                values
        //                  (
        //                    null
        //                    ,@Code
        //                    ,@CodeGroup
        //                  )";
        //    var id = db.Execute(sql, code);


        //    int codeId = db.ExecuteScalar<int>("SELECT LAST_INSERT_ID() as id ");
        //    code.CodeId = codeId;
        //}

        //[TestMethod]
        //public void AddNewProject()
        //{
        //    ProjectModel model = new ProjectModel();
        //    model.ProjectName = "Trade Manager 2nd Phase";
        //    model.Description = @"User requirements recap:
        //                        1.Allocation function enhancement; (complex & intergrated)
        //                        2.EQT inventory & forcast module;  (complex & intergrated)
        //                        3.Booking Stastic function enhancement;
        //                        4.Booking Edit function enhancement;
        //                        5. Booking Notice function;
        //                        6. SSE filing tool;
        //                        7. Report function;
        //                        8. Booker Rule function ;";
        //    model.Status = 1;
        //    model.Priority = 1;
        //    model.Users = "Edwin Nie & Joe Zhou";
        //    model.WorkTeam = "SPRC";
        //    model.RequestBy = "SPRC Trade/EQT";
        //    model.DifficultLevel = 1;
        //    model.ActualFinishDate = new DateTime(2018, 10, 1);
        //    model.CreationDate = DateTime.Now;
        //    model.ActualFinishDate = new DateTime(2018, 9, 21);
        //    MyProject.DAL.ProjectRepository repository = new DAL.ProjectRepository();
        //    repository.Add(model);
        //}

        //[TestMethod]
        //public void QueryTest()
        //{

        //    MyProject.DAL.ProjectRepository repository = new DAL.ProjectRepository();
        //    IDictionary<string, object> dic = new Dictionary<string, object>();

        //    dic.Add("ProjectName", "test");
        //    //dic.Add("Users", "Edwin Nie");
        //    var list = repository.Query(dic);
        //    //ProjectModel model = list.AsList()[0];
        //    //model.RequestBy = "Edwin Nie";
        //    //repository.Update(model);

        //}

        #endregion


        [TestMethod]
        public void AddNewTest()
        {

            


            MyProjectEF mp = new MyProjectEF();



            mp.uLibrary.ToList();

            mp.uUsers.Add(new uUser()
            {
                UserName = "cookienfy",
                Password = Base64Helper.Base64Encode("msc@123321"),
                Email = "edwin.nie@msc.com",
                CreationDate = DateTime.Now,
                EditDate = DateTime.Now,
                FirstName = "Edwin",
                LastName = "Nie",
                FullName = "Edwin Nie",
                LCV = false

            });
            mp.SaveChanges();

        }

        [TestMethod]
        public void CreateNewFunctions()
        {
            MyProjectEF ef = new MyProjectEF();

            uFunction fun = new uFunction();
            fun.FunName = "Dashboard";
            fun.FunTypeId = 1;
            fun.LCV = false;
            fun.CreationDate = DateTime.Now;
            fun.FunSeq = 0;
            ef.uFunctions.Add(fun);
            ef.SaveChanges();
        }

        [TestMethod]
        public void QueryTest()
        {
            using (var db = new MyProjectEF())
            {
                var list = db.uFunctions.Join(db.uCodes,
                      function => function.FunTypeId,
                      code => code.CodeId,
                      (f, c) => new
                      {
                          f.FunId,
                          f.FunName,
                          f.FunLink,
                          f.FunDesc,
                          f.FunParentId,
                          f.FunTypeId,
                          FunType = c.Code,
                          f.FunSeq,
                          f.CreationDate,
                          f.LCV
                      });
                foreach (var l in list)
                {
                    Console.Write(l);
                }
            }

        }

        [TestMethod]
        public void RepositoryTest()
        {



            UnitOfWork work = new UnitOfWork();

            var aa = work.LibraryRepository.DbSet.ToList();


            var queryable = work.FunctionRepository.DbSet.Join(work.CodeRepository.DbSet, f => f.FunTypeId, c => c.CodeId, (f, c) => new
            {
                f.FunId,
                f.FunName,
                f.FunLink,
                f.FunDesc,
                f.FunParentId,
                FunParent = f.FunParentId != null ? work.FunctionRepository.DbSet.FirstOrDefault(p => p.FunId == f.FunParentId).FunName : string.Empty,
                f.FunTypeId,
                FunType = c.Code,
                f.FunSeq,
                f.LCV
            });
            foreach (var l in queryable)
            {
                Console.Write(l);
            }

        }

        [TestMethod]
        public void GetCode()
        {
            using (MyProject.DAL.EF.MyProjectEF db = new DAL.EF.MyProjectEF())
            {
                var codeGroup = db.uCodeGroups.FirstOrDefault(p => p.CodeGroup.Equals("FunType"));
                var list = codeGroup.uCodes.ToList();


            }

        }

        [TestMethod]
        public void Respository()
        {
            UnitOfWork work = new UnitOfWork();
            work.LibraryRepository.Add(new uLibrary()
            {
                Doubt = "Test01",
                DoubtDesc = "Test01 Desc",
                ProcessDoubtWay = "Test01 Way",
                Contributor = "Edwin",
                ContributeDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                DifficultyTypeId = 1,
                Fun = "3PF->Add",
                FunTypeId = 1,
                LCV = false
            });

            work.SaveChanges();
            //using (MyProject.DAL.EF.MyProjectEF db = new DAL.EF.MyProjectEF())
            //{
            //    var ccc = db.uCodes.Where(p => p.CodeId == 1);
            //    foreach (var c in ccc)
            //    {

            //    }
            //}
            MyProjectEF db = new MyProjectEF();
            IRepository<uCode> rep = new Repository<uCode>(db);
            var list = rep.Query(p => p.CodeId == 1);
            foreach (var l in list)
            {

            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            MyProjectEF db = new MyProjectEF();

            FunctionRepository rep = new FunctionRepository(db);

            var aa = rep.Query(p => p.FunId == 2).FirstOrDefault();
            aa.FunDesc += "111232131";

            rep.Update(aa);

        }

        [TestMethod]
        public void TreeMenuHelpTest()
        {
            var aa = TreeMenuHelper.GetMenuJson();
            var b = 1;
        }

        [TestMethod]
        public void CryptographyManagerTest()
        {
            //System.Web.HttpUtility.UrlDecode()
            var aa = Base64Helper.Base64Decode("c2tkZmprc2o=");

            CryptographyManager cryptography = new CryptographyManager();

            string a = cryptography.Encrypt("1");

            string e = cryptography.Decrypt(a);

        }

        [TestMethod]
        public void GetUserList()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            var aa = unitOfWork.UserRepository.Query(null, null);
        }

    }
}

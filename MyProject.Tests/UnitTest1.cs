using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MyProject.DAL.Models;
using System.Collections;
using System.Collections.Generic;
using MyProject.Helpers;

namespace MyProject.Tests
{
    [TestClass]
    public class UnitTest1
    {
        string C_ConnectionStringName = "SqlCon";
        private IDbConnection GetConnection()
        {
            var str = System.Configuration.ConfigurationManager.ConnectionStrings[C_ConnectionStringName].ConnectionString;
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException("Connection string can not null or empty.");
            IDbConnection con = new MySqlConnection(str);

            return con;
        }

        [TestMethod]
        public void AddCode()
        {
            CodeModel code = new CodeModel();
            code.Code = "Low";
            code.CodeGroup = "Priority";



            IDbConnection db = GetConnection();

            string sql = @"insert into ucode
                          (
                            codeId
                            ,code
                            ,CodeGroup
                           )
                        values
                          (
                            null
                            ,@Code
                            ,@CodeGroup
                          )";
            var id = db.Execute(sql, code);


            int codeId = db.ExecuteScalar<int>("SELECT LAST_INSERT_ID() as id ");
            code.CodeId = codeId;
        }

        [TestMethod]
        public void AddNewProject()
        {
            ProjectModel model = new ProjectModel();
            model.ProjectName = "Trade Manager 2nd Phase";
            model.Description = @"User requirements recap:
                                1.Allocation function enhancement; (complex & intergrated)
                                2.EQT inventory & forcast module;  (complex & intergrated)
                                3.Booking Stastic function enhancement;
                                4.Booking Edit function enhancement;
                                5. Booking Notice function;
                                6. SSE filing tool;
                                7. Report function;
                                8. Booker Rule function ;";
            model.Status = 1;
            model.Priority = 1;
            model.Users = "Edwin Nie & Joe Zhou";
            model.WorkTeam = "SPRC";
            model.RequestBy = "SPRC Trade/EQT";
            model.DifficultLevel = 1;
            model.ActualFinishDate = new DateTime(2018, 10, 1);
            model.CreationDate = DateTime.Now;
            model.ActualFinishDate = new DateTime(2018, 9, 21);
            MyProject.DAL.ProjectRepository repository = new DAL.ProjectRepository();
            repository.Add(model);
        }

        [TestMethod]
        public void QueryTest()
        {

            MyProject.DAL.ProjectRepository repository = new DAL.ProjectRepository();
            IDictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("ProjectName", "test");
            //dic.Add("Users", "Edwin Nie");
            var list = repository.Query(dic);
            //ProjectModel model = list.AsList()[0];
            //model.RequestBy = "Edwin Nie";
            //repository.Update(model);

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

    }
}

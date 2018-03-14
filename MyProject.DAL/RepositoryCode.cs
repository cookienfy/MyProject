using MyProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MyProject.DAL
{
    public class CodeRepository : IRepository<CodeModel>
    {
        private const string C_ConnectionStringName = "SqlCon";

        public IDbConnection GetConnection()
        {
            var str = System.Configuration.ConfigurationManager.ConnectionStrings[C_ConnectionStringName].ConnectionString;
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException("Connection string can not null or empty.");
            IDbConnection con = new MySql.Data.MySqlClient.MySqlConnection(str);

            return con;
        }

        public IEnumerable<CodeModel> Query(IDictionary<string, object> dicParams)
        {
            string sql = @"select 
                              *
                            from 
                              ucode
                            where
                              1=1
                            and
                              CodeGroup=@CodeGroup";
            using (var con = GetConnection())
            {
                return con.Query<CodeModel>(sql, new { CodeGroup = dicParams["CodeGroup"].ToString() });
            }

        }


    }
}

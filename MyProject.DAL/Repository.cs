using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MyProject.DAL
{
    public abstract class Repository<T> : IRepository<T>, IDisposable
    {

        private const string C_ConnectionStringName = "SqlCon";

        protected IDbConnection Connection { get; set; }

        public Repository()
        {
            Connection = GetConnection();
        }

        public IDbConnection GetConnection()
        {
            var str = System.Configuration.ConfigurationManager.ConnectionStrings[C_ConnectionStringName].ConnectionString;
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException("Connection string can not null or empty.");
            IDbConnection con = new MySql.Data.MySqlClient.MySqlConnection(str);

            return con;
        }


        public abstract IEnumerable<T> Query(int Id);

        public abstract IEnumerable<T> Query(IDictionary<string,object> dicParams);

        public abstract void Add(T model);

        public abstract void Add(IEnumerable<T> models);

        public abstract void Delete(int id);

        public abstract void Delete(T model);

        public abstract void Update(T model);

        public void Dispose()
        {
            if (Connection != null)
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                    Connection.Close();
                Connection.Dispose();
            }
        }

       
    }
}

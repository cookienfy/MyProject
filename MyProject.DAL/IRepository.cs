using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DAL
{
    public interface IRepository<T>
    {

        //void Add(T model);

        //void Add(IEnumerable<T> models);

        //void Update(T model);

        //void Delete(int id);

        //void Delete(T model);

        IDbConnection GetConnection();

        IEnumerable<T> Query(IDictionary<string, object> dicParams);

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DAL
{
    public interface IRepository<T> where T : class
    {

        T Add(T model);

        IEnumerable<T> Add(IEnumerable<T> models);

        void Update(T model);

        void Delete(T model);

        IEnumerable<T> Query(Expression<Func<T, bool>> expWhere, Expression<Func<T, IOrderedQueryable>> expOrder = null);

    }



}

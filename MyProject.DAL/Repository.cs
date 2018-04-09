using MyProject.DAL.EF;
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
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {

        private DbContext _db;

        public virtual DbSet<T> DbSet { get; set; }

        public Repository()
        {
            CreateConnection();
        }

        public Repository(DbContext db)
        {
            this._db = db;
            DbSet = _db.Set<T>();
        }

        private void CreateConnection()
        {
            _db = new EF.MyProjectEF();
            DbSet = _db.Set<T>();
        }

        public virtual T Add(T model)
        {
            var m = DbSet.Add(model);
            //_db.SaveChanges();
            return m;
        }

        public virtual IEnumerable<T> Add(IEnumerable<T> models)
        {
            var ms = DbSet.AddRange(models);
            //_db.SaveChanges();
            return ms;
        }

        public virtual void Delete(T model)
        {
            if (_db.Entry(model).State == EntityState.Detached)
            {
                DbSet.Attach(model);
            }
            DbSet.Remove(model);
            //_db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db != null)
                _db.Dispose();
        }

        public virtual IEnumerable<T> Query(Expression<Func<T, bool>> expWhere, Expression<Func<T, IOrderedQueryable>> expOrder = null)
        {
            if (expOrder != null)
                return DbSet.Where(expWhere).OrderBy(expOrder);
            else
                return DbSet.Where(expWhere);
        }

        public virtual void Update(T model)
        {
            DbSet.Attach(model);
            _db.Entry(model).State = EntityState.Modified;
            //_db.SaveChanges();
        }

    }

    public class FunctionRepository : Repository<uFunction>
    {
        public FunctionRepository(DbContext db) : base(db) { }
    }


    public class CodeRepository : Repository<uCode>
    {
        public CodeRepository(DbContext db) : base(db)
        {

        }
    }
}

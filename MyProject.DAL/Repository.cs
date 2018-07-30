using MyProject.DAL.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DAL
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {

        private IDbContext _db;

        protected ObjectSet<T> _objectSet;

        public virtual DbSet<T> DbSet { get; set; }

        public Repository()
        {
            CreateConnection();
        }

        public Repository(IDbContext db)
        {
            this._db = db;
            DbSet = _db.Set<T>();

            _objectSet = ((IObjectContextAdapter)db).ObjectContext.CreateObjectSet<T>();
        }

        private void CreateConnection()
        {
            _db = new EF.MyProjectEF();
            DbSet = _db.Set<T>();
            _objectSet = ((IObjectContextAdapter)_db).ObjectContext.CreateObjectSet<T>();
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
            {
                _db.Dispose();
                _db = null;
            }
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

    public class FunctionRepository : Repository<uFunction>, IFunctionRepository
    {
        public FunctionRepository(IDbContext db) : base(db) { }
    }

    public interface IFunctionRepository : IRepository<uFunction>
    {

    }


    public class CodeRepository : Repository<uCode>
    {
        public CodeRepository(IDbContext db) : base(db)
        {

        }
    }

    public class LibraryRepository : Repository<uLibrary>
    {
        public LibraryRepository(IDbContext db) : base(db)
        {

        }
    }

    public class ContextRepository : Repository<uContext>
    {
        public ContextRepository(IDbContext db) : base(db)
        {

        }
    }

    public class UserRepository : Repository<uUser>
    {
        public UserRepository(IDbContext db) : base(db)
        {

        }

        public override IEnumerable<uUser> Query(Expression<Func<uUser, bool>> expWhere, Expression<Func<uUser, IOrderedQueryable>> expOrder = null)
        {
            if (expWhere != null)
                return base.Query(expWhere, expOrder);
            else
                return this.DbSet.ToList();
        }

    }


}

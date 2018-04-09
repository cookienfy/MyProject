using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DAL
{
    public class UnitOfWork : IDisposable
    {
        private DbContext _db;

        private CodeRepository _codeRepository;
        private FunctionRepository _functionRepository;

        public UnitOfWork()
        {
            _db = new EF.MyProjectEF();
            _codeRepository = new CodeRepository(_db);
            _functionRepository = new FunctionRepository(_db);
        }

        public CodeRepository CodeRepository
        {
            get => _codeRepository;
            set => _codeRepository = value;
        }

        public FunctionRepository FunctionRepository
        {
            get => _functionRepository;
            set => _functionRepository = value;
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }

        public void SaveChanges()
        {
            if (_db != null)
                _db.SaveChanges();
        }

    }
}

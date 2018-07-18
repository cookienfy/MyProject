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
        private LibraryRepository _libraryRepository;
        private FunctionRepository _functionRepository;
        private UserRepository _userRepository;
        private ContextRepository _contextRepository;


        public UnitOfWork()
        {
            _db = new EF.MyProjectEF();
            _codeRepository = new CodeRepository(_db);
            _functionRepository = new FunctionRepository(_db);
            _userRepository = new UserRepository(_db);
            _libraryRepository = new LibraryRepository(_db);
            _contextRepository = new ContextRepository(_db);
        }

        public DbContext DB { get; set; }

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

        public UserRepository UserRepository
        {
            get => _userRepository;
            set => _userRepository = value;
        }

        public LibraryRepository LibraryRepository
        {
            get => _libraryRepository;
            set => _libraryRepository = value;
        }

        public ContextRepository ContextRepository
        {
            get => _contextRepository;
            set => _contextRepository = value;
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

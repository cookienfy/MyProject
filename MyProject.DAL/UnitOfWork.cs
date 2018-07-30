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
        private IDbContext _dbContext;

        private CodeRepository _codeRepository;
        private LibraryRepository _libraryRepository;
        private FunctionRepository _functionRepository;
        private UserRepository _userRepository;
        private ContextRepository _contextRepository;


        public UnitOfWork()
        {
            _dbContext = new EF.MyProjectEF();
        }

        public DbContext DB { get; set; }

        public CodeRepository CodeRepository
        {
            get => _codeRepository ?? new CodeRepository(_dbContext);
        }

        public FunctionRepository FunctionRepository
        {
            get => _functionRepository ?? new FunctionRepository(_dbContext);
        }

        public UserRepository UserRepository
        {
            get => _userRepository ?? new UserRepository(_dbContext);
        }

        public LibraryRepository LibraryRepository
        {
            get => _libraryRepository ?? new LibraryRepository(_dbContext);
        }

        public ContextRepository ContextRepository
        {
            get => _contextRepository ?? new ContextRepository(_dbContext);
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        public void SaveChanges()
        {
            if (_dbContext != null)
                _dbContext.SaveChanges();
        }

    }
}

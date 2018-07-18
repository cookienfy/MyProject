using MyProject.DAL;
using MyProject.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.BLL
{
    public class LibraryBLL
    {

        private UnitOfWork work = new UnitOfWork();


        public IEnumerable<uLibrary> GetLibraries(string doubt)
        {
            return work.LibraryRepository.Query(p => p.Doubt.Contains(doubt) && !p.LCV);
        }

       

    }
}
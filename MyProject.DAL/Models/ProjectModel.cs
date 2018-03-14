using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.DAL.Models
{
    public class ProjectModel
    {
        [Key]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public CodeModel CodePriority { get; set; }

        public int DifficultLevel { get; set; }

        public string RequestBy { get; set; }

        public string Users { get; set; }

        public int Status { get; set; }


        public DateTime EstimateFinishDate { get; set; }

        public DateTime ActualFinishDate { get; set; }

        public string WorkTeam { get; set; }

        public string Remark { get; set; }

        public DateTime CreationDate { get; set; }

        public int LastUpdateBy { get; set; }

        public bool LCV { get; set; }
    }
}
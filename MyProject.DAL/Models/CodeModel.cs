using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.DAL.Models
{
    public class CodeModel
    {
        [Key]
        public int CodeId { get; set; }


        public string Code { get; set; }

        public string CodeGroup { get; set; }
    }
}
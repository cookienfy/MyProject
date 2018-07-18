using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DAL.EF
{
    [Table("uContext")]
    public class uContext
    {
        [Key]
        public int ContextId { get; set; }

        public int LibraryId { get; set; }

        [StringLength(100)]
        public string FileName { get; set; }

        [StringLength(10)]
        public string Extension { get; set; }

        public byte[] Content { get; set; }

      
    }
}

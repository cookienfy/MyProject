namespace MyProject.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("uFunction")]
    public partial class uFunction
    {
        [Key]
        public int FunId { get; set; }

        [StringLength(20)]
        public string FunName { get; set; }

        [StringLength(100)]
        public string FunLink { get; set; }

        [StringLength(200)]
        public string FunDesc { get; set; }

        public int? FunParentId { get; set; }

        public int? FunTypeId { get; set; }

        public int? FunSeq { get; set; }

        public DateTime? CreationDate { get; set; }

        public bool? LCV { get; set; }

        [StringLength(20)]
        public string FunPic { get; set; }
    }
}

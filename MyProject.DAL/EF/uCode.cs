namespace MyProject.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("uCode")]
    public partial class uCode
    {
        [Key]
        public int CodeId { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        public int? CodeGroupId { get; set; }

        public DateTime? CreationDate { get; set; }

        public bool? LCV { get; set; }

        public virtual uCodeGroup uCodeGroup { get; set; }
    }
}

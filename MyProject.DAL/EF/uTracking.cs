namespace MyProject.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("uTracking")]
    public partial class uTracking
    {
        [Key]
        public int TrackingId { get; set; }

        public int? ProjectId { get; set; }

        [StringLength(10)]
        public string VerNo { get; set; }

        [StringLength(500)]
        public string Content { get; set; }

        public DateTime? UpdatingDate { get; set; }

        [StringLength(20)]
        public string User { get; set; }

        public int? LastUpdateBy { get; set; }

        public bool? LCV { get; set; }

        public virtual uProject uProject { get; set; }
    }
}

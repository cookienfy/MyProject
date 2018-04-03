namespace MyProject.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("uProject")]
    public partial class uProject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public uProject()
        {
            uTrackings = new HashSet<uTracking>();
        }

        [Key]
        public int ProjectId { get; set; }

        [StringLength(100)]
        public string ProjectName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int? Priority { get; set; }

        public int? DifficultLevel { get; set; }

        [StringLength(50)]
        public string RequestBy { get; set; }

        [StringLength(100)]
        public string Users { get; set; }

        public int? Status { get; set; }

        public DateTime? EstimateFinishDate { get; set; }

        public DateTime? ActualFinishDate { get; set; }

        [StringLength(50)]
        public string WorkTeam { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? LastUpdateBy { get; set; }

        public bool? LCV { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<uTracking> uTrackings { get; set; }
    }
}

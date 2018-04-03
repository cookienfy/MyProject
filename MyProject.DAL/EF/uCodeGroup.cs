namespace MyProject.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("uCodeGroup")]
    public partial class uCodeGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public uCodeGroup()
        {
            uCodes = new HashSet<uCode>();
        }

        [Key]
        public int CodeGroupId { get; set; }

        [StringLength(30)]
        public string CodeGroup { get; set; }

        public DateTime? CreationDate { get; set; }

        public bool? LCV { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<uCode> uCodes { get; set; }
    }
}

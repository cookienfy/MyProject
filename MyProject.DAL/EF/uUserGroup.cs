namespace MyProject.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("uUserGroup")]
    public partial class uUserGroup
    {
        [Key]
        public int UserGroupId { get; set; }

        [StringLength(30)]
        public string UserGroupName { get; set; }

        public DateTime? CreationDate { get; set; }

        public bool? LCV { get; set; }
    }
}

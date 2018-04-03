namespace MyProject.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("uUser")]
    public partial class uUser
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(30)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(10)]
        public string FirstName { get; set; }

        [StringLength(10)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string FullName { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? EditDate { get; set; }

        public DateTime? LastLogon { get; set; }

        public bool? LCV { get; set; }
    }
}

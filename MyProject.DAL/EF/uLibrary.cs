using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace MyProject.DAL.EF
{
    [Table("uLibrary")]
    public class uLibrary
    {
        public uLibrary()
        {
            //uContexts = new HashSet<uContext>();
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int LibraryId { get; set; }

        /// <summary>
        /// 功能
        /// </summary>
        [StringLength(50)]
        public string Fun { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public int FunTypeId { get; set; }

        /// <summary>
        /// 疑问，问题
        /// </summary>
        [StringLength(50)]
        public string Doubt { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        [StringLength(500)]
        public string DoubtDesc { get; set; }

        /// <summary>
        /// 解决办法
        /// </summary>
        [StringLength(500)]
        public string ProcessDoubtWay { get; set; }

        /// <summary>
        /// 难度级别
        /// </summary>
        public int DifficultyTypeId { get; set; }

        /// <summary>
        /// 贡献者
        /// </summary>
        [StringLength(100)]
        public string Contributor { get; set; }

        /// <summary>
        /// 贡献日期
        /// </summary>
        public DateTime? ContributeDate { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }

        public bool LCV { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        //public virtual ICollection<uContext> uContexts { get; set; }
    }

}

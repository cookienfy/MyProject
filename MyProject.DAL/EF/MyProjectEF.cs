namespace MyProject.DAL.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyProjectEF : DbContext
    {
        public MyProjectEF()
            : base("name=MyProjectEF1")
        {
        }

        public virtual DbSet<uCode> uCodes { get; set; }
        public virtual DbSet<uCodeGroup> uCodeGroups { get; set; }
        public virtual DbSet<uFunction> uFunctions { get; set; }
        public virtual DbSet<uProject> uProjects { get; set; }
        public virtual DbSet<uTracking> uTrackings { get; set; }
        public virtual DbSet<uUser> uUsers { get; set; }
        public virtual DbSet<uUserGroup> uUserGroups { get; set; }

        public virtual DbSet<uContext> uContexts { get; set; }

        public virtual DbSet<uLibrary> uLibrary { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<uCode>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<uCodeGroup>()
                .Property(e => e.CodeGroup)
                .IsUnicode(false);

            modelBuilder.Entity<uFunction>()
                .Property(e => e.FunName)
                .IsUnicode(false);

            modelBuilder.Entity<uFunction>()
                .Property(e => e.FunLink)
                .IsUnicode(false);

            modelBuilder.Entity<uFunction>()
                .Property(e => e.FunDesc)
                .IsUnicode(false);

            modelBuilder.Entity<uProject>()
                .Property(e => e.ProjectName)
                .IsUnicode(false);

            modelBuilder.Entity<uProject>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<uProject>()
                .Property(e => e.RequestBy)
                .IsUnicode(false);

            modelBuilder.Entity<uProject>()
                .Property(e => e.Users)
                .IsUnicode(false);

            modelBuilder.Entity<uProject>()
                .Property(e => e.WorkTeam)
                .IsUnicode(false);

            modelBuilder.Entity<uProject>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<uTracking>()
                .Property(e => e.VerNo)
                .IsUnicode(false);

            modelBuilder.Entity<uTracking>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<uTracking>()
                .Property(e => e.User)
                .IsUnicode(false);

            modelBuilder.Entity<uUser>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<uUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<uUser>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<uUser>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<uUser>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<uUser>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<uUserGroup>()
                .Property(e => e.UserGroupName)
                .IsUnicode(false);
        }
    }


}

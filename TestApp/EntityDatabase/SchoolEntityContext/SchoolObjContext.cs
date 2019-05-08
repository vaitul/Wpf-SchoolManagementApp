using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.EntityDatabase.DomainClasses;

namespace TestApp.EntityDatabase.SchoolEntityContext
{
    public class SchoolObjContext : DbContext
    {
        public SchoolObjContext()
            : base("School_DB")
        {

        }

        public DbSet<DomainClasses.Student> Students { get; set; }
        public DbSet<DomainClasses.Standard> Standards { get; set; }
        public DbSet<DomainClasses.AllMarks> AllMarks { get; set; }
        public DbSet<DomainClasses.Subject> Subjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Mapping.StudentMap());
            modelBuilder.Configurations.Add(new Mapping.StandardMap());
            modelBuilder.Configurations.Add(new Mapping.AllMarksMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}

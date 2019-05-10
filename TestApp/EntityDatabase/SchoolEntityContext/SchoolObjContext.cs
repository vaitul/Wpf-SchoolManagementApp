using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
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

            var conf = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type=>!string.IsNullOrEmpty(type.Namespace))
                .Where(type=>type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)).ToArray();
            //.Where(type => type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)).ToArray();

            modelBuilder.Configurations.Add(new Mapping.StudentMap());
            modelBuilder.Configurations.Add(new Mapping.StandardMap());
            modelBuilder.Configurations.Add(new Mapping.AllMarksMap());
            modelBuilder.Configurations.Add(new Mapping.SubjectMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}

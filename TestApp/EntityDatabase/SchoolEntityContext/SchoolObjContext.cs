using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using SchoolManagementSys.EntityDatabase.Mapping;

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
                .Where(type => type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>))
                .Select(type=>Activator.CreateInstance(type)).ToArray();

            foreach(dynamic instance in conf)
            {
                modelBuilder.Configurations.Add(instance);
            }
            //modelBuilder.Configurations.Add(new Mapping.StudentMap());
            //modelBuilder.Configurations.Add(new Mapping.StandardMap());
            //modelBuilder.Configurations.Add(new Mapping.AllMarksMap());
            //modelBuilder.Configurations.Add(new SubjectMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}

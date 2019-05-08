using System.Data.Entity.ModelConfiguration;
using TestApp.EntityDatabase.DomainClasses;

namespace TestApp.EntityDatabase.Mapping
{
    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            this.ToTable("Students");
            this.HasKey(t => t.StudentId);
            this.Property(t => t.FirstName);
            this.Property(t => t.MiddleName);
            this.Property(t => t.LastName);
            this.Property(t => t.Age);
            this.Property(t => t.City);
            this.HasRequired(t => t.Standard).WithMany().HasForeignKey(t => t.StandardId);
        }
    }
}

using System.Data.Entity.ModelConfiguration;
using TestApp.EntityDatabase.DomainClasses;

namespace SchoolManagementSys.EntityDatabase.Mapping
{
    public class SubjectMap : EntityTypeConfiguration<Subject>
    {
        public SubjectMap()
        {
            this.ToTable("Subjects");
            this.HasKey(x => x.Id);
            this.Property(x => x.Name);
            this.HasRequired(x => x.Standard).WithMany().HasForeignKey(x => x.StandardId);
        }
    }
}

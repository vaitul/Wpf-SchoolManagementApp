using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.EntityDatabase.Mapping
{
    public class AllMarksMap : EntityTypeConfiguration<DomainClasses.AllMarks>
    {
        public AllMarksMap()
        {
            this.ToTable("AllMarks");
            this.HasKey(t => t.Id);
            this.HasRequired(t => t.Student).WithMany().HasForeignKey(t => t.StudentId).WillCascadeOnDelete(true);
            this.HasRequired(t => t.Standard).WithMany().HasForeignKey(t => t.StandardId).WillCascadeOnDelete(false);
            this.HasRequired(t => t.Subject).WithMany().HasForeignKey(t => t.SubjectId).WillCascadeOnDelete(false);
            this.Property(t => t.Mark);
        }
    }
}

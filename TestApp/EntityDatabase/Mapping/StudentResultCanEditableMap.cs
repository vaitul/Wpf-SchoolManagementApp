using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.EntityDatabase.DomainClasses;

namespace TestApp.EntityDatabase.Mapping
{
    public class StudentResultCanEditableMap : EntityTypeConfiguration<StudentResultCanEditable>
    {
        public StudentResultCanEditableMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.StudentId);
            this.Property(t => t.StandardId);
            //this.Property(t => t.CanEditable);
        }
    }
}

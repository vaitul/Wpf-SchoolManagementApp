using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.EntityDatabase.Mapping
{
    public class StandardMap : EntityTypeConfiguration<DomainClasses.Standard>
    {
        public StandardMap()
        {
            this.ToTable("Standards");
            this.HasKey(t => t.StandardId);
            this.Property(t => t.StandardName);
        }
    }
}

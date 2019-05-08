using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.EntityDatabase.DomainClasses
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int StandardId { get; set; }
        public Standard Standard { get; set; }
    }
}

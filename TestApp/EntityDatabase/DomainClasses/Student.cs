using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.EntityDatabase.DomainClasses
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public byte[] Doc { get; set; }
        public string DocType { get; set; }

        public int StandardId { get; set; }
        public Standard Standard { get; set; }
    }
}

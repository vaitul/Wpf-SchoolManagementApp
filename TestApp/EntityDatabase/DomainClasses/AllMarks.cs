using System.Collections.Generic;

namespace TestApp.EntityDatabase.DomainClasses
{
    public class AllMarks
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int StandardId { get; set; }
        public Standard Standard { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int Mark { get; set; }

        //public bool CanEditable { get; set; }
    }
}

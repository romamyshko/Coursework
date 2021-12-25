using System;
using System.Collections.Generic;

#nullable disable

namespace DBControl.Models
{
    public partial class Subject
    {
        public Subject()
        {
            StudentsSubjects = new HashSet<StudentSubject>();
        }

        public int SubjectId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StudentSubject> StudentsSubjects { get; set; }
    }
}

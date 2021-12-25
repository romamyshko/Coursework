using System;
using System.Collections.Generic;

#nullable disable

namespace DBControl.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentsSubjects = new HashSet<StudentSubject>();
        }

        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public string Fullname { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<StudentSubject> StudentsSubjects { get; set; }
    }
}

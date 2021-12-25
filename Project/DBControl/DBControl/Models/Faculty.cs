using System;
using System.Collections.Generic;

#nullable disable

namespace DBControl.Models
{
    public partial class Faculty
    {
        public Faculty()
        {
            Courses = new HashSet<Course>();
        }

        public int FacultyId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}

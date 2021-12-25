using System;
using System.Collections.Generic;

#nullable disable

namespace DBControl.Models
{
    public partial class Course
    {
        public Course()
        {
            Groups = new HashSet<Group>();
        }

        public int CourseId { get; set; }
        public int FacultyId { get; set; }
        public string Name { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}

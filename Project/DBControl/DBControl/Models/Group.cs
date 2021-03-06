using System;
using System.Collections.Generic;

#nullable disable

namespace DBControl.Models
{
    public partial class Group
    {
        public Group()
        {
            Students = new HashSet<Student>();
        }

        public int GroupId { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }

        public virtual Course Course { get; set; }
        public virtual IEnumerable<Student> Students { get; set; }
    }
}

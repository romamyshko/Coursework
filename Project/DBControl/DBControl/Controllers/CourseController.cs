using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBControl.Models;

namespace DBControl.Controllers
{
    internal class CourseController
    {
        private readonly UniversityContext _context;

        public CourseController(UniversityContext context)
        {
            _context = context;
        }

        public int Insert(Course course)
        {
            try
            {
                _context.Add(course);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public int Update(Course updatedCourse)
        {
            try
            {
                _context.Update(updatedCourse);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public Course Get(int id)
        {
            return _context.Courses.Find(id);
        }

        public int Delete(int id)
        {
            try
            {
                Course course = _context.Courses.Find(id);

                if (DeleteAllGroupsByCourseId(course.CourseId) != -1)
                {
                    _context.Courses.Remove(course);
                }

                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        private int DeleteAllGroupsByCourseId(int courseId)
        {
            try
            {
                IQueryable<Group> groupsList = _context.Groups.Where(id => id.CourseId == courseId); // id.CourseId or id.Course.CourseId ??

                foreach (var group in groupsList)
                {
                    _context.Groups.Remove(group);
                }

                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }
    }
}

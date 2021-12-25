using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBControl.Models;

namespace DBControl.Controllers
{
    internal class FacultyController
    {
        private readonly UniversityContext _context;

        public FacultyController(UniversityContext context)
        {
            _context = context;
        }

        public int Insert(Faculty faculty)
        {
            try
            {
                _context.Add(faculty);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public int Update(Faculty updatedFaculty)
        {
            try
            {
                _context.Update(updatedFaculty);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public Faculty Get(int id)
        {
            return _context.Faculties.Find(id);
        }

        public int Delete(int id)
        {
            try
            {
                Faculty faculty = _context.Faculties.Find(id);

                if (DeleteAllCoursesByFacultyId(faculty.FacultyId) != -1)
                {
                    _context.Faculties.Remove(faculty);
                }

                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        private int DeleteAllCoursesByFacultyId(int facultyId)
        {
            try
            {
                IQueryable<Course> coursesList = _context.Courses.Where(id => id.FacultyId == facultyId); // id.FacultyId or id.Faculty.FacultyId ??

                foreach (var course in coursesList)
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
    }
}

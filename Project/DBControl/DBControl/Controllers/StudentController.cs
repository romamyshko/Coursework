using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBControl.Models;

namespace DBControl.Controllers
{
    internal class StudentController
    {
        private readonly UniversityContext _context;

        public StudentController(UniversityContext context)
        {
            _context = context;
        }

        public int Insert(Student student)
        {
            try
            {
                _context.Add(student);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public List<Student> GetAllByGroupId(int groupId)
        {
            return _context.Students.Where(s => s.GroupId == groupId).ToList();
        }

        public int GetId(string fullname)
        {
            var numId = _context.Students.FirstOrDefault(s => s.Fullname == fullname)?.StudentId;
            if (numId != null)
                return (int) numId;

            return -1;
        }

        public int Update(Student updatedStudent)
        {
            try
            {
                _context.Update(updatedStudent);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public Student Get(int id)
        {
            return _context.Students.Find(id);
        }

        public int Delete(int id)
        {
            try
            {
                Student student = _context.Students.Find(id);

                if (DeleteAllStudentsSubjectsByStudentId(student.StudentId) != -1)
                {
                    _context.Students.Remove(student);
                }
                    
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        private int DeleteAllStudentsSubjectsByStudentId(int studentId)
        {
            try
            {
                IQueryable<StudentSubject> studentSubjectList =
                    _context.StudentsSubjects.Where(id =>
                        id.StudentId == studentId); // id.StudentId or id.Student.StudentId ??

                foreach (var studentSubject in studentSubjectList)
                {
                    _context.StudentsSubjects.Remove(studentSubject);
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

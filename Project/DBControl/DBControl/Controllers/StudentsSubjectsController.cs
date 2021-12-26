using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBControl.Models;

namespace DBControl.Controllers
{
    public class StudentsSubjectsController
    {
        private readonly UniversityContext _context;

        public StudentsSubjectsController(UniversityContext context)
        {
            _context = context;
        }

        public List<StudentSubject> GetByStudentId(int studentId)
        {
            return _context.StudentsSubjects.Where(s => s.StudentId == studentId).ToList();
        }

        public int Insert(StudentSubject studentSubject)
        {
            try
            {
                _context.Add(studentSubject);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public int Update(StudentSubject updatedStudentSubject)
        {
            try
            {
                _context.Update(updatedStudentSubject);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public StudentSubject Get(int id)
        {
            return _context.StudentsSubjects.Find(id);
        }
            
        public float GetAverageMark(int studentId)
        {
            var sbs = GetByStudentId(studentId);
            int marks = 0;
            int count = 0;
            foreach (var studentSubject in sbs)
            {
                marks += studentSubject.Mark;
                count++;
            }

            return (float) marks / count;
        }

        public int Delete(int id)
        {
            try
            {
                StudentSubject studentSubject = _context.StudentsSubjects.Find(id);
                _context.StudentsSubjects.Remove(studentSubject);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }
    }
}

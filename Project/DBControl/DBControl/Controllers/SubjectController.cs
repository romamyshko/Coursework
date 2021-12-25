using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBControl.Models;

namespace DBControl.Controllers
{
    internal class SubjectController
    {
        private readonly UniversityContext _context;

        public SubjectController(UniversityContext context)
        {
            _context = context;
        }

        public int Insert(Subject subject)
        {
            try
            {
                _context.Add(subject);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public int Update(Subject updatedSubject)
        {
            try
            {
                _context.Update(updatedSubject);
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public Subject Get(int id)
        {
            return _context.Subjects.Find(id);
        }

        public int Delete(int id)
        {
            try
            {
                Subject subject = _context.Subjects.Find(id);

                if (DeleteAllStudentsSubjectsBySubjectId(subject.SubjectId) != -1)
                {
                    _context.Subjects.Remove(subject);
                }

                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        private int DeleteAllStudentsSubjectsBySubjectId(int subjectId)
        {
            try
            {
                IQueryable<StudentSubject> studentSubjectList = _context.StudentsSubjects.Where(id => id.SubjectId == subjectId); // the same <-|

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

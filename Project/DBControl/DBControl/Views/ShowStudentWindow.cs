using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DBControl.Controllers;
using DBControl.Models;

namespace DBControl.Views
{
    internal class ShowStudentWindow
    {
        private readonly UniversityContext _context;
        private readonly StudentsSubjectsController _studentSubject;
        private readonly StudentController _student;
        private readonly SubjectController _subject;

        public ShowStudentWindow(UniversityContext context)
        {
            _context = context;
            _studentSubject = new StudentsSubjectsController(_context);
            _student = new StudentController(_context);
            _subject = new SubjectController(_context);
        }

        public void Run(string[] fullName = null)
        {
            string[] fullname = fullName;
            while (true)
            {
                if (fullName != null)
                {
                    break;
                }

                Console.Clear();
                Console.WriteLine("Enter name and surname in such format: Steve Jobs");

                fullname = GetFullname(Console.ReadLine());

                if (fullname.Length == 2)
                {
                    break;
                }
            }

            Console.Clear();
            Console.WriteLine($"Name: '{fullname[0]}' surname: '{fullname[1]}'");

            int id = _student.GetId(fullname[0] + " " + fullname[1]);

            if (id == -1)
            {
                Console.WriteLine($"Student: {fullname[0]} {fullname[1]} was not found.");
                System.Threading.Thread.Sleep(4000);
                return;
            }

            var sbs = _studentSubject.GetByStudentId(id).ToList();

            if (!sbs.Any())
            {
                Console.WriteLine($"Student: '{fullname[0]} {fullname[1]}' do not have any marks.");
                System.Threading.Thread.Sleep(4000);
                return;
            }

            var subjects = new List<Subject>();
            var marks = new List<int>();

            foreach (var studentSubject in sbs)
            {
                var subj = _subject.Get(studentSubject.SubjectId);
                subjects.Add(subj);
                marks.Add(studentSubject.Mark);

                Console.WriteLine($"Subjects: {subj.Name}, Mark: {studentSubject.Mark}");
            }

            Console.ReadLine();
        }

        private string[] GetFullname(string input)
        {
            return input.Split(' ');
        }
    }
}

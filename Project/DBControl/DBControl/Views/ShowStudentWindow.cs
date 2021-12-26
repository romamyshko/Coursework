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
using DiagramGenerator;

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

            var subjectsMarks = new Dictionary<string, double>();

            foreach (var studentSubject in sbs)
            {
                var subj = _subject.Get(studentSubject.SubjectId);
                if (subjectsMarks.ContainsKey(subj.Name))
                {
                    subjectsMarks[subj.Name] = (subjectsMarks[subj.Name] + studentSubject.Mark) / 2;
                }
                else
                {
                    subjectsMarks.Add(subj.Name, studentSubject.Mark);
                }

                Console.WriteLine($"Subjects: {subj.Name}, Mark: {studentSubject.Mark}");
            }

            Console.WriteLine("To generate diagram enter 'g'");
            string input = "";
            while (input != "g")
            {
                input = Console.ReadLine();
                if (input == "exit")
                {
                    return;
                }

                Console.WriteLine("Write 'g' or 'exit':");
            }

            Console.WriteLine($"Diagram was generated to path '{GenerateDiagram(subjectsMarks)}'");

            Console.WriteLine("\r\nPress any key to return back...");
            ConsoleKeyInfo key = Console.ReadKey();
        }

        private string[] GetFullname(string input)
        {
            return input.Split(' ');
        }

        private string GenerateDiagram(Dictionary<string, double> subjectsMarks)
        {
            var generator = new Generator(subjectsMarks);
            return generator.GenerateDiagram("D:/subjects_average_mark_diagram.png");
        }
    }
}

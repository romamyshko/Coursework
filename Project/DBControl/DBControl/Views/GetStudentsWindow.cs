using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DBControl.Controllers;
using DBControl.Models;

namespace DBControl.Views
{
    internal class GetStudentsWindow
    {
        private readonly UniversityContext _context;
        private readonly StudentController _student;
        private readonly StudentsSubjectsController _studentSubject;
        private readonly GroupController _group;

        public GetStudentsWindow(UniversityContext context)
        {
            _context = context;
            _student = new StudentController(_context);
            _studentSubject = new StudentsSubjectsController(_context);
            _group = new GroupController(_context);
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter the group to search." +
                                  "\r\nYou can filter by marks using >/>=, </<=, == and a number [60, 100]." +
                                  "\r\nType in such format: KP-03 >= 85");

                string[] filter = Console.ReadLine()?.Split(' ');
                string group = "";
                if (filter != null)
                {
                    group = EnterGroup(filter[0]);
                    if (group == "back")
                    {
                        return;
                    }

                    if (group == null)
                    {
                        continue;
                    }
                }

                List<Student> students = _student.GetAllByGroupId(_group.Get(group).GroupId); 
                List<StudentSubject> sbs = new List<StudentSubject>();

                string[] operands = new[] { ">", "<", ">=", "<=", "==" };

                int num = -1;

                if (filter is {Length: 3})
                {
                    if (!operands.Contains(filter[1]))
                    {
                        continue;
                    }

                    bool isNum = int.TryParse(filter[2], out num);
                    if (!isNum)
                    {
                        continue;
                    }
                }

                Console.WriteLine("\r\n******Results*of*Search******\r\n");

                foreach (var student in students)
                {
                    sbs = _studentSubject.GetByStudentId(student.StudentId);
                    int marks = 0;
                    int count = 0;
                    foreach (var studentSubject in sbs)
                    {
                        marks += studentSubject.Mark;
                        count++;
                    }

                    float avrMark = (float) marks / count;

                    if (num != -1)
                    {
                        if (filter != null)
                            switch (filter[1])
                            {
                                case ">":
                                    if (!(avrMark > num)) continue;
                                    break;
                                case "<":
                                    if (!(avrMark < num)) continue;
                                    break;
                                case ">=":
                                    if (!(avrMark >= num)) continue;
                                    break;
                                case "<=":
                                    if (!(avrMark <= num)) continue;
                                    break;
                                case "==":
                                    if ((int) avrMark != num) continue;
                                    break;
                            }
                    }

                    var addInfo = $" AVG mark: {avrMark}";
                    Console.WriteLine($"Student\t{student.Fullname} \t[{group}]{addInfo}");
                }

                Console.WriteLine("\r\nPress any key to return back...");
                ConsoleKeyInfo key = Console.ReadKey();
                return;
            }
        }

        private string EnterGroup(string group)
        {
            if (group == "back")
            {
                return "back";
            }
            if (_context.Groups.Count(elem => elem.Name.Equals(group)) != 0)
            {
                return group;
            }

            return null;
        }

    }
}

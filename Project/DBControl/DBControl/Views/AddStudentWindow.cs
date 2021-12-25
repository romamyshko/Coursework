using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBControl.Controllers;
using DBControl.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DBControl.Views
{
    internal class AddStudentWindow
    {
        private readonly UniversityContext _context;
        private readonly FacultyController _faculty;
        private readonly StudentController _student;
        private readonly CourseController _course;
        private readonly GroupController _group;

        public AddStudentWindow(UniversityContext context)
        {
            _context = context;
            _faculty = new FacultyController(_context);
            _student = new StudentController(_context);
            _course = new CourseController(_context);
            _group = new GroupController(_context);
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("You can write 'back' to return to the main window.");
                Console.WriteLine("Write some information about student below:");
                Console.Write("Full name: ");
                string fullname = Console.ReadLine();
                if (fullname == "back")
                {
                    return;
                }

                string group = EnterGroup();
                if (group == "back")
                {
                    return;
                }

                Group g = _group.Get(group);
                g.Course = _course.Get(g.CourseId);
                g.Course.Faculty = _faculty.Get(g.Course.FacultyId);
                if (fullname != null && g is {Course: { }})
                {
                    int idStudent = _student.Insert(new Student()
                        {Fullname = fullname.Split(' ')[0] + " " + fullname.Split(' ')[1], GroupId = g.GroupId, Group = g});
                    ShowStudentWindow w = new ShowStudentWindow(_context);
                    w.Run(fullname.Split(' '));
                    return;
                }
            }
        }

        private string EnterGroup()
        {
            while (true)
            {
                Console.Write("Group: ");
                string group = Console.ReadLine();
                if (group == "back")
                {
                    return "back";
                }
                if (_context.Groups.Count(elem => elem.Name.Equals(group)) != 0)
                {
                    return group;
                }
            }
        }

        private int ChooseWorkingOption(string input)
        {
            if (int.TryParse(input, out var number) && number is >= 1 and <= 4)
            {
                switch (number)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }

                return 1;
            }

            return input is "exit" ? 0 : -1;
        }
    }
}

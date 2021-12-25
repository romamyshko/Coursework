using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBControl.Models;

namespace DBControl.Views
{
    internal class StartWindow
    {
        private readonly UniversityContext _context;

        public StartWindow(UniversityContext context)
        {
            _context = context;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose working option:\r\n\t[1] Add a new student\r\n\t[2]" +
                                  " Find the student\r\n\t[3]" +
                                  " Get students [filter]\r\n\r\nWrite 'exit' to end program.");
                
                if (ChooseWorkingOption(Console.ReadLine()) == 0)
                {
                    return;
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
                        var addingStudentWindow = new AddStudentWindow(_context);
                        addingStudentWindow.Run();
                        break;
                    case 2:
                        var studentWindow = new ShowStudentWindow(_context);
                        studentWindow.Run();
                        break;
                    case 3:
                        var getStudents = new GetStudentsWindow(_context);
                        getStudents.Run();
                        break;
                }

                return 1;
            }

            return input is "exit" ? 0 : -1;
        }
    }
}

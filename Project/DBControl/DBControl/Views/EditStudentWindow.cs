using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBControl.Models;

namespace DBControl.Views
{
    internal class EditStudentWindow
    {
        private readonly UniversityContext _context;

        public EditStudentWindow(UniversityContext context)
        {
            _context = context;
        }
    }
}

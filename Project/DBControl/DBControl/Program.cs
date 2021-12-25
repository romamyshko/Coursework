using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DBControl.Controllers;
using DBControl.Models;
using DBControl.Views;

namespace DBControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load sample data
            var sampleData = new MLModel1.ModelInput()
            {
                Column1 = @"John",
                Gender = @"M",
                DOB = @"05/04/1988",
                Maths = 55F,
                Physics = 45F,
                Chemistry = 56F,
                English = 87F,
                Biology = 21F,
                Economics = 52F,
                Civics = 65F,
            };

            //Load model and predict output
            var result = MLModel1.Predict(sampleData);

            Console.WriteLine(result);
            Console.ReadLine();

            const string jsFilepath = "../../../../../../data/application.json";
            DbConnectionInfo connection = GetConnectionFromJson(jsFilepath);

            UniversityContext context = new UniversityContext(connection);
            //GenerateStudentSubject(context);
            StartWindow startWindow = new StartWindow(context);
            startWindow.Run();
        }

        private static DbConnectionInfo GetConnectionFromJson(string filePathToJson)
        {
            return JsonSerializer.Deserialize<DbConnectionInfo>(new StreamReader(filePathToJson).ReadToEnd());
        }

        private static void GenerateStudentSubject(UniversityContext context)
        {
            var students = context.Students.ToArray();
            var subjects = context.Subjects.ToArray();
            Random random = new Random();

            var sbc = new StudentsSubjectsController(context);
            var num = new int[] { 67, 99, 85, 90, 69, 74, 96, 79, 87, 75, 71, 89, 65, 94, 98, 69, 60, 74, 87, 100} ;
            foreach (var student in students)
            {
                for (int i = 0; i < 10; i++)
                {
                    var subject = subjects[i];
                    StudentSubject sb = new StudentSubject();
                    sb.SubjectId = subject.SubjectId;
                    sb.Subject = subject;
                    sb.StudentId = student.StudentId;
                    sb.Student = student;
                    sb.Mark = num[i];
                    sbc.Insert(sb);
                }
            }
        }
    }
}

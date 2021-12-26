using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DBControl.Models;
using DBControl.Controllers;
using Npgsql;

namespace DataGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionInfo connection = GetConnectionFromJson("../../../../../../data/application.json");
            Run(connection);
            GenerateStudentSubject(new UniversityContext(new DbConnectionInfo() {Connection = connection.Connection }));
        }

        static ConnectionInfo GetConnectionFromJson(string filePathToJson)
        {
            return JsonSerializer.Deserialize<ConnectionInfo>(new StreamReader(filePathToJson).ReadToEnd());
        }

        static void Run(ConnectionInfo connectionInfo)
        {
            List<Faculty> faculties = JsonSerializer.Deserialize<List<Faculty>>(new StreamReader("../../../../../../data/faculties.json").ReadToEnd());

            List<string> courses = new List<string>();
            List<string> subjects = new List<string>();

            GetSeperateCsvData("../../../../../../data/courses_subjects.csv", ref courses, ref subjects);

            List<string> names = new List<string>();
            List<string> surnames = new List<string>();

            GetSeperateCsvData("../../../../../../data/names_surnames.csv", ref names, ref surnames);



            NpgsqlConnection connection = new NpgsqlConnection(connectionInfo.Connection);

            connection.Open();

            RunSqlCommands(faculties, courses, subjects, names, surnames, connection);

            connection.Close();
        }

        static void RunSqlCommands(List<Faculty> faculties, List<string> courses,
            List<string> subjects, List<string> names, List<string> surnames, NpgsqlConnection connection)
        {
            var r = new Random();

            var facultiesArr = faculties.ToArray();
            var coursesArr = courses.ToArray();
            var subjectsArr = subjects.ToArray();
            var namesArr = names.ToArray();
            var surnamesArr = surnames.ToArray();

            for (int i = 0; i < facultiesArr.Length; i++)
            {
                var insertFaculty = new NpgsqlCommand($"INSERT INTO faculties (name) VALUES('{facultiesArr[i].Name}') RETURNING faculty_id", connection);
                int facultyId = (int)insertFaculty.ExecuteScalar()!;

                for (int j = 0; j < 3; j++)
                {
                    var insertCourse = new NpgsqlCommand($"INSERT INTO courses (faculty_id, name) VALUES({facultyId}, '{coursesArr[j + i * 3]}') RETURNING course_id", connection);
                    int courseId = (int)insertCourse.ExecuteScalar()!;


                    var insertSubject = new NpgsqlCommand($"INSERT INTO subjects (name) VALUES('{subjectsArr[j + i * 3]}') RETURNING subject_id", connection);
                    int subjectId = (int)insertSubject.ExecuteScalar()!;

                    for (int k = 0; k < 3; k++)
                    {
                        var insertGroup = new NpgsqlCommand($"INSERT INTO groups (course_id, name) VALUES({courseId}, '{(char)r.Next(66, 90)}{(char)r.Next(65, 90)}-{r.Next(11, 90)}') RETURNING group_id", connection);
                        int groupId = (int)insertGroup.ExecuteScalar()!;

                        for (int l = 0; l < 20; l++)
                        {
                            var insertStudent = new NpgsqlCommand($"INSERT INTO students (group_id,fullname) VALUES({groupId}, '{namesArr[r.Next(namesArr.Length - 1)] + " " + surnamesArr[r.Next(surnamesArr.Length - 1)]}') RETURNING student_id", connection);
                            int studentId = (int)insertStudent.ExecuteScalar()!;

                            var insertStudentSubject = new NpgsqlCommand($"INSERT INTO students_subjects (student_id, subject_id, mark) VALUES({studentId}, {subjectId}, {r.Next(60, 100)})", connection);
                            insertStudentSubject.ExecuteScalar();
                        }
                    }
                }
            }
        }

        static void GetSeperateCsvData(string filepath, ref List<string> list1, ref List<string> list2)
        {
            string[] csv = File.ReadAllLines(filepath);

            foreach (var s in csv)
            {
                string[] splitted = s.Split(',', 2);

                if (splitted[0] != "")
                {
                    list1.Add(splitted[0]);
                }

                if (splitted[1] != "")
                {
                    list2.Add(splitted[1]);
                }
            }
        }

        static void GenerateStudentSubject(UniversityContext context)
        {
            var students = context.Students.ToArray();
            var subjects = context.Subjects.ToArray();
            Random random = new Random();

            var sbc = new StudentsSubjectsController(context);
            var num = new int[] { 67, 99, 85, 90, 69, 74, 96, 79, 87, 75 };
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DBControl.Controllers;
using DBControl.Models;
using DBControl.Views;
using DiagramGenerator;

namespace DBControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string jsFilepath = "../../../../../../data/application.json";
            DbConnectionInfo connection = GetConnectionFromJson(jsFilepath);

            UniversityContext context = new UniversityContext(connection);

            StartWindow startWindow = new StartWindow(context);
            startWindow.Run();
        }

        private static DbConnectionInfo GetConnectionFromJson(string filePathToJson)
        {
            return JsonSerializer.Deserialize<DbConnectionInfo>(new StreamReader(filePathToJson).ReadToEnd());
        }

        private float PredictMark()
        {
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

            var result = MLModel1.Predict(sampleData);

            return result.Score;
        }
    }
}

using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DiagramGenerator
{
    public class Generator
    {
        private readonly List<string> _labels = new List<string>();
        private readonly List<double> _marks = new List<double>();
        private int width = 600;
        private int height = 600;

        public Generator()
        {

        }

        public Generator(List<string> students, List<double> avrMarks)
        {
            _labels = students;
            _marks = avrMarks;
        }

        public Generator(Dictionary<string, double> subjectsMarks)
        {
            foreach (var subjectsMark in subjectsMarks)
            {
                _labels.Add(subjectsMark.Key);
                _marks.Add(subjectsMark.Value);
            }

            width = 1200;
        }

        public string GenerateDiagram(string filePath)
        {
            var plt = new ScottPlot.Plot(width, height);

            double[] values = _marks.ToArray();
            string[] labels = _labels.ToArray();
            var bar = plt.AddBar(values);
            bar.Orientation = Orientation.Horizontal;
            plt.YTicks(labels);
            plt.SetAxisLimits();

            return plt.SaveFig(filePath);
        }

        public void Generate()
        {
            var plt = new ScottPlot.Plot(600, 400);

            // These are the dat we will plot with a linear X scale but log Y scale
            double[] xs = { 1, 2, 3};
            double[] ys = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120 };

            // Plot the Log10 of all the Y values
            double[] logYs1 = {64, 109, 100};
            var scatter1 = plt.AddScatter(xs, logYs1, lineWidth: 2, markerSize: 10);
            double[] logYs2 = { 53, 86, 80 };
            var scatter2 = plt.AddScatter(xs, logYs2, lineWidth: 2, markerSize: 10);

            // Use a custom formatter to control the label for each tick mark
            //static string logTickLabels(double y) => Math.Pow(10, y).ToString("N0");
            //plt.YAxis.TickLabelFormat(logTickLabels);

            // Use log-spaced minor tick marks and grid lines to make it more convincing
            plt.YAxis.MinorLogScale(true);
            plt.YAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));
            plt.YAxis.MinorGrid(true, Color.FromArgb(20, Color.Black));
            plt.XAxis.MajorGrid(true, Color.FromArgb(80, Color.Black));

            // Set the axis limits manually to ensure edges terminate at desirable locations
            

            plt.SaveFig("D:/asis_log.png");
        }
    }
}
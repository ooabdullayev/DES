using System;
using System.IO;

namespace DES
{
    abstract class ResultsRenderer //обеспечивает место вывода результатов
    {
        //вывод одной коллеции результатов
        abstract public void RenderResults(Results results, string tableName);
        //вывод соотношения нескольких коллекций результатов
        abstract public void RenderResultsRelation(Results results1, Results results2,
            Results results3, string tableName);
        //проверка на совместимость коллекций результатов
        static protected void CheckConformity(Results results1, Results results2, Results results3,
            out sbyte commonCount, out double[] commonSetOfPoints, out sbyte commonDimension)
        {
            if (results1.Dimension != results2.Dimension
                || results1.Dimension != results3.Dimension)
                throw new ResultsRendererException("Results do not have the same dimension of results.");

            commonDimension = results1.Dimension;

            if (results1.Number != results2.Number || results1.Number != results3.Number)
                throw new ResultsRendererException("Results do not have the same count.");

            commonCount = checked((sbyte)results1.Number);
            commonSetOfPoints = new double[commonCount];

            for (sbyte i = 0; i < commonCount; i++)
                if (results1[i].t != results2[i].t || results1[i].t != results3[i].t)
                    throw new ResultsRendererException("Results do not have the same set of points.");
                else
                    commonSetOfPoints[i] = results1[i].t;

        }
    }
    class ResultsRendererException : Exception
    {
        public ResultsRendererException() : base("Incorrect results.") { }
        public ResultsRendererException(string message) : base(message) { }
    }
    class ConsoleRenderer : ResultsRenderer //класс для вывода результатов в консоль
    {
        public override void RenderResults(Results results, string tableName)
        {
            Console.WriteLine();
            Console.WriteLine(tableName);
            Console.WriteLine("eps = {0}", results.epsilon);
            Console.WriteLine();
            Console.WriteLine("| {0,7} | {1,19} | {2,15} |", 't', "y(t)", "delta");

            foreach (Result result in results)
            {
                Console.WriteLine("| {0,7} | {1,19} | {2,15} |", ' ', ' ', ' ');
                Console.WriteLine("| {0,7:f2} | {1,19:E10} | {2,15:E6} |", result.t, result.y[0], result.errGlobal);

                for (sbyte i = 1; i < result.Dimension; i++)
                    Console.WriteLine("| {0,7} | {1,19:E10} | {2,15} | ", ' ', result.y[i], ' ');
            }
        }
        public override void RenderResultsRelation(Results results1, Results results2,
            Results results3, string tableName)
        {
            sbyte count;
            double[] t;
            sbyte dimension;

            CheckConformity(results1, results2, results3, out count, out t, out dimension);

            Console.WriteLine();
            Console.WriteLine(tableName);
            Console.WriteLine("eps1 = {0}, eps2 = {1}, eps3 = {2}",
                results1.epsilon, results2.epsilon, results3.epsilon);
            Console.WriteLine();
            Console.WriteLine("| {0,7} | {1,12} | {2,12} | {3,12} | {4,12} | {5,12} |",
                't', "y1-y2", "y2-y3", "y1-y2/y2-y3", "del1/del2", "del2/del3");
            for (sbyte i = 0; i < count; i++)
            {
                Vector yCol1, yCol2, yCol3;
                yCol1 = results1[i].y - results2[i].y;
                yCol2 = results2[i].y - results3[i].y;
                yCol3 = yCol1 / yCol2;
                double errCol1, errCol2;
                errCol1 = results1[i].errGlobal / results2[i].errGlobal;
                errCol2 = results2[i].errGlobal / results3[i].errGlobal;
                Console.WriteLine("| {0,7} | {1,12} | {2,12} | {3,12} | {4,12} | {5,12} |", ' ', ' ', ' ', ' ', ' ', ' ');
                Console.WriteLine("| {0,7:f2} | {1,12:E2} | {2,12:E2} | {3,12:E2} | {4,12:E2} | {5,12:E2} |",
                    t[i], yCol1[0], yCol2[0], yCol3[0], errCol1, errCol2);
                for (sbyte j = 1; j < dimension; j++)
                    Console.WriteLine("| {0,7} | {1,12:E2} | {2,12:E2} | {3,12:E2} | {4,12} | {5,12} |",
                        ' ', yCol1[j], yCol2[j], yCol3[j], ' ', ' ');

            }
        }
    }
    class LaTeXRenderer : ResultsRenderer //класс для вывода результатов в latex файл
    {
        string outputFileName;
        string tableWidth;
        public override void RenderResults(Results results, string tableName)
        {
            StreamWriter file = new StreamWriter(outputFileName);

            file.WriteLine("\\begin{center}");
            file.WriteLine("\\begin{tabu} to " + tableWidth + "\\textwidth {|X[l]|X[r]|X[r]|}");
            file.WriteLine("\\multicolumn{3}{c}{" + tableName + "} \\\\");
            file.WriteLine("\\hline");

            file.WriteLine("\\multicolumn{3}{|c|}{");
            file.WriteLine("$\\varepsilon$ = " + results.epsilon + "} \\\\");
            file.WriteLine("\\hline");
            file.WriteLine("$t$ & $y(t)$ & $\\delta$ \\\\");
            file.WriteLine("\\hline");

            foreach (Result result in results)
            {
                file.WriteLine("{0:f2} & {1:E10} & {2:E6} \\\\", result.t, result.y[0], result.errGlobal);

                for (sbyte i = 1; i < result.Dimension; i++)
                    file.WriteLine(" & {0:E10} & \\\\", result.y[i]);

                file.WriteLine("\\hline");
            }

            file.WriteLine("\\end{tabu}");
            file.WriteLine("\\end{center}");
            file.Close();
        }
        public override void RenderResultsRelation(Results results1, Results results2,
            Results results3, string tableName)
        {
            StreamWriter file = new StreamWriter(outputFileName);

            sbyte count;
            double[] t;
            sbyte dimension;

            CheckConformity(results1, results2, results3, out count, out t, out dimension);

            file.WriteLine("\\begin{center}");
            file.WriteLine("\\begin{tabu} to " + tableWidth + "\\textwidth {|X[l]|X[r]|X[r]|X[r]|X[r]|X[r]|}");
            file.WriteLine("\\multicolumn{6}{c}{" + tableName + "} \\\\");
            file.WriteLine("\\hline");

            file.WriteLine("\\multicolumn{6}{|c|}{");
            file.WriteLine("$\\varepsilon_1$ = " + results1.epsilon);
            file.WriteLine(", $\\varepsilon_2$ = " + results2.epsilon);
            file.WriteLine(", $\\varepsilon_3$ = " + results3.epsilon + "} \\\\");
            file.WriteLine("\\hline");

            file.WriteLine("$t$ & $y_1-y_2$ & $y_2-y_3$ & $\\frac{y_1-y_2}{y_2-y_3}$ & ");
            file.WriteLine("$\\delta_1/\\delta_2$ & $\\delta_2/\\delta_3$ \\\\ [0.1cm]");
            file.WriteLine("\\hline");

            for (sbyte i = 0; i < count; i++)
            {
                Vector yCol1, yCol2, yCol3;
                yCol1 = results1[i].y - results2[i].y;
                yCol2 = results2[i].y - results3[i].y;
                yCol3 = yCol1 / yCol2;
                double errCol1, errCol2;
                errCol1 = results1[i].errGlobal / results2[i].errGlobal;
                errCol2 = results2[i].errGlobal / results3[i].errGlobal;
                file.WriteLine("{0:f2} & {1:E2} & {2:E2} & {3:E2} & {4:E2} & {5:E2} \\\\",
                    t[i], yCol1[0], yCol2[0], yCol3[0], errCol1, errCol2);
                for (sbyte j = 1; j < dimension; j++)
                    file.WriteLine(" & {0:E2} & {1:E2} & {2:E2} & & \\\\", yCol1[j], yCol2[j], yCol3[j]);

                file.WriteLine("\\hline");
            }
            file.WriteLine("\\end{tabu}");
            file.WriteLine("\\end{center}");
            file.Close();
        }
        public LaTeXRenderer(string outputFileName)
        {
            this.outputFileName = outputFileName;
            tableWidth = "0.96";
        }
    }
}

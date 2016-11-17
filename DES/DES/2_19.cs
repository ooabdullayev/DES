using System;

namespace DES
{
    class _2_19
    {
        static Vector f(double t, Vector y)
        {
            double y0 = y[1];
            double y1 = -0.5 * Math.Sin(y[0]) + Math.Sin(t);

            return new Vector(y0, y1);
        }
        static double Lambda(double t, Vector y)
        {
            return 0.5 - 0.25 * Math.Cos(y[0]);
        }
        static Conditions MakeConditions(double alpha)
        {
            return new Conditions(0, new Vector(0.0, alpha));
        }
        static double GetComponent(Vector y)
        {
            return y[1];
        }
        public static void Solve(string fileName, sbyte numOfPoints, double alpha0, double alpha)
        {
            sbyte numOfEquations = 2;
            double tLast = Math.PI / 2;
            double epsilon1 = 1e-7;
            double epsilon2 = 1e-9;
            double epsilon3 = 1e-11;

            //создаем задачу с неполными начальными условиями
            IncompleteConditionsProblem problem =
                new IncompleteConditionsProblem(tLast, MakeConditions, GetComponent, numOfEquations, f, Lambda);

            //создаем поставщик данных метода
            IMethodProvider provider = new FileMethodProvider(fileName);

            //создаем метод из данных, полученных от поставщика
            Method method = new Method(provider);

            //Преобразуем нашу задачу к классической задаче Коши при помощи полученного метода
            ClassicProblem clProblem = problem.ConvertToClassic(method, epsilon3, alpha0, alpha);

            //решаем полученные задачи с разной степенью точности
            Results results1 = clProblem.Solve(method, numOfPoints, epsilon1);
            Results results2 = clProblem.Solve(method, numOfPoints, epsilon2);
            Results results3 = clProblem.Solve(method, numOfPoints, epsilon3);

            //создаем визуализатор результатов в консоль
            ResultsRenderer renderer = new ConsoleRenderer();
            ResultsRenderer rendererTex1 = new LaTeXRenderer("table1.tex");
            ResultsRenderer rendererTex2 = new LaTeXRenderer("table2.tex");
            ResultsRenderer rendererTex3 = new LaTeXRenderer("table3.tex");
            ResultsRenderer rendererTex4 = new LaTeXRenderer("table4.tex");

            //выводим результаты в консоль
            renderer.RenderResults(results1, "Таблица 1.");
            renderer.RenderResults(results2, "Таблица 2.");
            renderer.RenderResults(results3, "Таблица 3.");

            rendererTex1.RenderResults(results1, "Таблица 1.");
            rendererTex2.RenderResults(results2, "Таблица 2.");
            rendererTex3.RenderResults(results3, "Таблица 3.");

            //выводим соотношения результатов
            renderer.RenderResultsRelation(results1, results2, results3, "Таблица 4.");
            rendererTex4.RenderResultsRelation(results1, results2, results3, "Таблица 4.");

            //Всё!
        }
    }
}

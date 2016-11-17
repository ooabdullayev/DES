using System;

namespace DES
{
    class _2_2
    {
        static Vector f(double t, Vector y)
        {
            double y0 = y[1];
            double y1 = 2 * y[0] * (2 * y[0] * y[0] - 1);
            y1 *= 1 + y[0] * y[0] - y[0] * y[0] * y[0] * y[0] - y[2];
            double y2 = y[3];
            double y3 = 1 + y[0] * y[0] - y[0] * y[0] * y[0] * y[0] - y[2];

            return new Vector(y0, y1, y2, y3);
        }
        static double Lambda(double t, Vector y)
        {
            double u, v;
            double result;

            u = 2 * (6 * y[0] * y[0] - 1) * (1 + y[0] * y[0] - y[0] * y[0] * y[0] * y[0] - y[2]);
            u -= 4 * y[0] * y[0] * (2 * y[0] * y[0] - 1) * (2 * y[0] * y[0] - 1);
            u = (u + 1) / 2;

            v = y[0] * (1 - 2 * y[0] * y[0]);

            result = u * u + 2 * v * v;
            result += Math.Sqrt(result * result - 4 * v * v * v * v);
            result /= 2;
            return Math.Sqrt(result);

        }
        public static void Solve(double alpha, sbyte numOfPoints, string fileName1, string fileName2)
        {
            sbyte numOfEquations = 4;
            double t0 = 0;
            double tLast = 50;
            Vector y0 = new Vector(alpha, 0, alpha * alpha * (1 - alpha * alpha), 0);
            double epsilon1 = 1e-7;
            double epsilon2 = 1e-9;
            double epsilon3 = 1e-11;
            Conditions conditions = new Conditions(t0, y0);

            //создаем классическую задачу Коши
            ClassicProblem myProblem = new ClassicProblem(conditions, tLast, numOfEquations, f, Lambda);

            //создаем поставщиков разных методов
            IMethodProvider provider1 = new FileMethodProvider(fileName1);
            IMethodProvider provider2 = new FileMethodProvider(fileName2);

            //создаем методы
            Method method1 = new Method(provider1);
            Method method2 = new Method(provider2);

            //получаем результаты решения нашего уравнения для трёх разных допустимых погрешностей
            Results results11 = myProblem.Solve(method1, numOfPoints, epsilon1);
            Results results12 = myProblem.Solve(method1, numOfPoints, epsilon2);
            Results results13 = myProblem.Solve(method1, numOfPoints, epsilon3);

            Results results21 = myProblem.Solve(method2, numOfPoints, epsilon1);
            Results results22 = myProblem.Solve(method2, numOfPoints, epsilon2);
            Results results23 = myProblem.Solve(method2, numOfPoints, epsilon3);
            
            //создаем места вывода наших результатов
            ResultsRenderer laTeXRenderer1 = new LaTeXRenderer("tableEps1.tex");
            ResultsRenderer laTeXRenderer2 = new LaTeXRenderer("tableEps2.tex");
            ResultsRenderer laTeXRenderer3 = new LaTeXRenderer("tableEps3.tex");
            ResultsRenderer laTeXRendererRelation = new LaTeXRenderer("tableRelation.tex");

            ResultsRenderer consoleRenderer = new ConsoleRenderer();

            //выводим резултаты
            laTeXRenderer1.RenderResults(results11, "Таблица 1");
            laTeXRenderer2.RenderResults(results12, "Таблица 2");
            laTeXRenderer3.RenderResults(results13, "Таблица 3");
            laTeXRendererRelation.RenderResultsRelation(results11, results12, results13, "Таблица 4");

            Console.WriteLine();
            Console.WriteLine("By Felberg method.");

            consoleRenderer.RenderResults(results11, "Таблица 1");
            consoleRenderer.RenderResults(results12, "Таблица 2");
            consoleRenderer.RenderResults(results13, "Таблица 3");
            consoleRenderer.RenderResultsRelation(results11, results12, results13, "Таблица 4");

            Console.WriteLine();
            Console.WriteLine("By Dorman - Prince method.");

            consoleRenderer.RenderResults(results21, "Таблица 1");
            consoleRenderer.RenderResults(results22, "Таблица 2");
            consoleRenderer.RenderResults(results23, "Таблица 3");
            consoleRenderer.RenderResultsRelation(results21, results22, results23, "Таблица 4");
        }
    }
}

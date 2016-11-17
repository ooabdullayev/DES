using System;

namespace DES
{
    class _2_4
    {
        static double value = Math.PI;
        static Vector f(double t, Vector y)
        {
            double G = -1 + y[2] * y[2] * (1 - Math.Cos(y[0]) / 3) / 2;
            G -= y[2] * y[2] * y[2] * y[2] * Math.Cos(y[0]) / 48;

            double G1 = y[2] * y[2] * Math.Sin(y[0]) / 6;
            G1 *= 1 + y[2] * y[2] / 8;

            double G3 = 1 + y[2] * y[2] / 4;
            G3 = 1 - Math.Cos(y[0]) * G3 / 3;
            G3 *= y[2];

            double F1 = G1 + G * G1 / (y[2] * y[2]);

            double F3 = G3 * y[2] - G;
            F3 *= G / (y[2] * y[2] * y[2]);

            double y0 = y[1];

            double y1 = -F1;

            double y2 = y[3];

            double y3 = -F3;

            return new Vector(y0, y1, y2, y3);
        }
        static double Lambda(double t, Vector y)
        {
            double G = -1 + y[2] * y[2] * (1 - Math.Cos(y[0]) / 3) / 2;
            G -= y[2] * y[2] * y[2] * y[2] * Math.Cos(y[0]) / 48;

            double G1 = y[2] * y[2] * Math.Sin(y[0]) / 6;
            G1 *= 1 + y[2] * y[2] / 8;

            double G3 = 1 + y[2] * y[2] / 4;
            G3 = 1 - Math.Cos(y[0]) * G3 / 3;
            G3 *= y[2];

            double G11 = 1 + y[2] * y[2] / 8;
            G11 *= Math.Cos(y[0]) * y[2] * y[2] / 6;

            double G13 = 1 + y[2] * y[2] / 4;
            G13 *= y[2] * Math.Sin(y[0]) / 3;

            double G31 = 1 + y[2] * y[2] / 4;
            G31 *= y[2] * Math.Sin(y[0]) / 3;

            double G33 = 1 + y[2] * y[2] / 4;
            G33 *= Math.Cos(y[0]) / 3;
            G33 = 1 - G33 - y[2] * y[2] * Math.Cos(y[0]) / 6;

            double F11 = G11;
            F11 += (G1 * G1 + G * G11) / (y[2] * y[2]);
            F11 = -F11;

            double F13 = G13;
            F13 += (G3 * G1 + G * G13) * y[2] - 2 * G * G1;
            F13 /= y[2] * y[2] * y[2];
            F13 = -F13;

            double F31 = G3 * (y[2] * G3 - G) + G * (G31 * y[2] - G1);
            F31 /= y[2] * y[2] * y[2];
            F31 = -F31;

            double F33 = G3 * (G33 * y[2] - G) + G * G33 * y[2];
            F33 *= y[2];
            F33 -= 3 * G * (G3 * y[2] - G);
            F33 /= y[2] * y[2] * y[2] * y[2];
            F33 = -F33;

            double _F11 = (F11 + 1) / 2;
            double _F13 = F13 / 2;
            double _F31 = F31 / 2;
            double _F33 = (F33 + 1) / 2;

            double u = _F11 * _F11 + _F13 * _F13 + _F31 * _F31 + _F33 * _F33;
            double v = _F11 * _F11 * _F33 * _F33 + _F13 * _F13 * _F31 * _F31;

            double result = Math.Sqrt(u * u - 4 * v);
            result = (u + result) / 2;
            return Math.Sqrt(result);
        }
        static double AdjustStep(Vector y, Vector yChange, double eps, double h)
        {
            if ((Math.Abs(y[0]) <= value - eps && Math.Abs(y[0] + yChange[0]) >= value + eps) ||
                (Math.Abs(y[0]) >= value + eps && Math.Abs(y[0] + yChange[0]) <= value - eps))
            {
                return Math.Abs((value - Math.Abs(y[0])) * h / yChange[0]);
            }
            return h;
        }
        static bool IsPointReached(Vector y, double eps)
        {
            if (Math.Abs(y[0]) > value - eps && Math.Abs(y[0]) < value + eps)
                return true;
            return false;
        }
        public static void Solve(sbyte numOfPoints, string fileName)
        {
            sbyte numOfEquations = 4;
            double t0 = 0;
            Vector y0 = new Vector(Math.PI / 2, 0, Math.Sqrt(2), 0);
            double epsilon1 = 1e-7;
            double epsilon2 = 1e-9;
            double epsilon3 = 1e-11;

            Conditions conditions = new Conditions(t0, y0);

            
            //создаем задачу с неизвестной конечной точкой
            UnknownPointProblem problem =
                new UnknownPointProblem(conditions, numOfEquations, f, Lambda, AdjustStep, IsPointReached);

            //создаем поставщик данных метода
            IMethodProvider provider = new FileMethodProvider(fileName);

            //создаем метод из данных, полученных от поставщика
            Method method = new Method(provider);

            //Преобразуем нашу задачу к классической задаче Коши при помощи полученного метода
            ClassicProblem clProblem = problem.ConvertToClassic(method, epsilon3);
            
            //ClassicProblem clProblem = new ClassicProblem(conditions, 10.0, 4, f, Lambda);
            //решаем полученные задачи с разной степенью точности
            Results results1 = clProblem.Solve(method, numOfPoints, epsilon1);
            Results results2 = clProblem.Solve(method, numOfPoints, epsilon2);
            Results results3 = clProblem.Solve(method, numOfPoints, epsilon3);

            //создаем визуализатор результатов в консоль
            ResultsRenderer renderer = new ConsoleRenderer();
            ResultsRenderer latex1 = new LaTeXRenderer("tbl1.tex");
            ResultsRenderer latex2 = new LaTeXRenderer("tbl2.tex");
            ResultsRenderer latex3 = new LaTeXRenderer("tbl3.tex");
            ResultsRenderer latex4 = new LaTeXRenderer("tbl4.tex");

            //выводим результаты в консоль
            renderer.RenderResults(results1, "Таблица 1.");
            renderer.RenderResults(results2, "Таблица 2.");
            renderer.RenderResults(results3, "Таблица 3.");

            latex1.RenderResults(results1, "Таблица 1.");
            latex2.RenderResults(results2, "Таблица 2.");
            latex3.RenderResults(results3, "Таблица 3.");

            //выводим соотношения результатов
            renderer.RenderResultsRelation(results1, results2, results3, "Таблица 4.");
            latex4.RenderResultsRelation(results1, results2, results3, "Таблица 4.");

            //Всё!
        }
    }
}

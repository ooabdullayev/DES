using System;
using System.IO;

namespace DES
{
    interface IMethodProvider //данный интерфейс обеспечивает источник данных для метода
    {
        void GetMethod(out sbyte numOfSteps, out sbyte methodOrder, out double[] y1, out double[] y1_, out double[][] a, out double[] c);
    }
    //Данный класс обеспечивает считывание параметров метода из файла.
    //Файл на первой строке должен содержать два целых числа: число стадий и порядок метода.
    //Дальше идет ступенчатый двумерный массив отвечающий значениям aij.
    //И последние две строки файла содержат значения для y и y_ соответственно.
    class FileMethodProvider : IMethodProvider
    {
        string fileName;
        public void GetMethod(out sbyte numOfSteps, out sbyte methodOrder, out double[] y1, out double[] y1_, out double[][] a, out double[] c)
        {
            StreamReader file = new StreamReader(fileName);

            //считываем первую строку файла, которая должна содержать два числа
            string[] currentLine = file.ReadLine().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            numOfSteps = sbyte.Parse(currentLine[0]);
            methodOrder = sbyte.Parse(currentLine[1]);

            //нету методов с количеством стадий меньше 3 или порядком меньше 1
            if (numOfSteps < 3 || methodOrder < 1) throw new FileMethodProviderException("Incorrect values in file.");

            a = new double[numOfSteps - 1][];
            for (sbyte i = 0; i < numOfSteps - 1; i++)
                a[i] = new double[i + 1];
            y1 = new double[numOfSteps];
            y1_ = new double[numOfSteps];
            c = new double[numOfSteps - 1];

            //в следующих строках содержится, собственно, сама таблица
            for (sbyte i = 0; i < numOfSteps - 1; i++)
            {
                currentLine = file.ReadLine().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                for (sbyte j = 0; j < i + 1; j++)
                    a[i][j] = GetValue(currentLine[j]);
            }

            currentLine = file.ReadLine().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            for (sbyte j = 0; j < numOfSteps; j++)
            {
                y1[j] = GetValue(currentLine[j]);
            }

            currentLine = file.ReadLine().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            for (sbyte j = 0; j < numOfSteps; j++)
            {
                y1_[j] = GetValue(currentLine[j]);
            }

            currentLine = file.ReadLine().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            for (sbyte j = 0; j < numOfSteps - 1; j++)
            {
                c[j] = GetValue(currentLine[j]);
            }

            file.Close();
        }
        static double GetValue(string strVal) //метод возращает десятичное число (не в виде дроби)
        {
            string[] fraction = strVal.Split('/');
            if (fraction.Length == 1) return Double.Parse(fraction[0]);
            else if (fraction.Length == 2) return Double.Parse(fraction[0]) / Double.Parse(fraction[1]);
            else throw new FileMethodProviderException("Incorrect values in file.");
        }
        public FileMethodProvider(string fileName)
        {
            this.fileName = fileName;
        }

    }
    class FileMethodProviderException : Exception
    {
        public FileMethodProviderException() : base("Incorrect file.") { }
        public FileMethodProviderException(string message) : base(message) { }
    }


}

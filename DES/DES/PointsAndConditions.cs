using System;
using System.Collections;
using System.Collections.Generic;

namespace DES
{
    class Points : IEnumerable<double> //множество точек в которых получаем решение
    {
        readonly double[] points;
        public sbyte Number //количество точек
        {
            get
            {
                return checked((sbyte)points.Length);
            }
        }
        public Points(double tMin, double tMax, sbyte numOfPoints)
        {
            //бессмысленно искать решение при отсутствии точек
            if (numOfPoints < 1) throw new PointsException("Incorrect number of points.");

            points = new double[numOfPoints];

            double h = (tMax - tMin) / numOfPoints;

            points[0] = tMin + h;
            for (sbyte i = 1; i < numOfPoints; i++)
            {
                points[i] = points[i - 1] + h;
            }
        }
        public double this[sbyte index]
        {
            get
            {
                return points[index];
            }
        }
        public IEnumerator<double> GetEnumerator()
        {
            return new PointsEnumerator(this); //использовать родной enumerator для массивов
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    class PointsEnumerator : IEnumerator<double> //обеспечивает итерацию по точкам
    {
        sbyte position;
        readonly Points points;
        public PointsEnumerator(Points points)
        {
            this.points = points;
            position = -1;
        }

        public double Current
        {
            get
            {
                return points[position];
            }
        }
        public bool MoveNext()
        {
            position++;
            return (position < points.Number);
        }
        public void Reset()
        {
            position = -1;
        }
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        public void Dispose() { }
    }
    class PointsException : Exception
    {
        public PointsException() : base("Incorrect set of points.") { }
        public PointsException(string message) : base(message) { }
    }
    class Conditions
    {
        public readonly double t0;
        public readonly Vector y0;
        public Conditions(double t0, Vector y0)
        {
            this.t0 = t0;
            this.y0 = y0;
        }
    }
}

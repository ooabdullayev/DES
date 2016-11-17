using System;
using System.Collections;
using System.Collections.Generic;

namespace DES
{
    class Result //инкапсулирует результат метода
    {
        readonly public double t; //точка, в которой вычислен результат
        readonly public Vector y; //собственно сам результат
        readonly public double errGlobal; //глобальная погрешность
        public Result(double t, Vector y, double errGlobal)
        {
            this.t = t;
            this.y = y;
            this.errGlobal = errGlobal;
        }
        public sbyte Dimension
        {
            get
            {
                return y.Dimension;
            }
        }
    }
    class Results : IEnumerable<Result> //для хранения коллекции результатов
    {
        readonly public double epsilon;
        readonly Result[] results;
        public sbyte Number
        {
            get
            {
                return checked((sbyte)results.Length);
            }
        }
        public Results(List<Result> results, double epsilon)
        {
            foreach (Result result in results)
            {
                if (result.Dimension != results[0].Dimension)
                    throw new ResultsException("Results do not have the same dimensions.");
            }
            this.epsilon = epsilon;
            this.results = results.ToArray();
        }
        public Result this[sbyte index]
        {
            get
            {
                return results[index];
            }
        }
        public sbyte Dimension
        {
            get
            {
                return this[0].Dimension;
            }
        }
        public IEnumerator<Result> GetEnumerator()
        {
            return new ResultsEnumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    class ResultsEnumerator : IEnumerator<Result>
    {
        sbyte position;
        readonly Results results;
        public ResultsEnumerator(Results results)
        {
            this.results = results;
            position = -1;
        }

        public Result Current
        {
            get
            {
                return results[position];
            }
        }
        public bool MoveNext()
        {
            position++;
            return (position < results.Number);
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
    class ResultsException : Exception
    {
        public ResultsException() : base("Incorrect results.") { }
        public ResultsException(string message) : base(message) { }
    }
}

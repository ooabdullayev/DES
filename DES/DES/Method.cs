using System;

namespace DES
{
    class Method //инкапсулирует s-стадийный явный метод Рунге-Кутты
    {
        public readonly sbyte numOfSteps; //число стадий метода Рунге-Кутты
        public readonly sbyte methodOrder; //порядок метода
        //следующие три переменные хранят значения таблицы конкретного метода
        public readonly double[] y1;
        public readonly double[] y1_; //соотвутсвует "игрик один с крышкой"
        public readonly double[][] a;
        public readonly double[] c;
        public Method(IMethodProvider methodProvider)
        {
            methodProvider.GetMethod(out numOfSteps, out methodOrder, out y1, out y1_, out a, out c);
        }
    }
    class MethodException : Exception
    {
        public MethodException() : base("Incorrect method.") { }
        public MethodException(string message) : base(message) { }
    }
}

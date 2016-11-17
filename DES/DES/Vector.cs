using System;

namespace DES
{
    public class Vector
    {
        readonly double[] components; //для хранения компонентов вектора
        public sbyte Dimension
        {
            get
            {
                return checked((sbyte)components.Length);
            }
        }
        public Vector(sbyte dimension)
        {
            if (dimension < 1) throw new VectorException("Incorrect dimension of vector.");
            components = new double[dimension];
        }
        public Vector(params double[] components)
        {
            if (components.Length < 1) throw new VectorException("Incorrect dimension of vector.");
            this.components = (double[])components.Clone();
        }
        public double this[sbyte index]
        {
            get
            {
                return components[index];
            }

        }
        public static Vector operator +(Vector op1, Vector op2)
        {
            if (op1.Dimension != op2.Dimension) throw new VectorException("Dimensions of vectors in + operator are not the same.");
            double[] result = new double[op1.Dimension];
            for (sbyte i = 0; i < result.Length; i++)
            {
                result[i] = op1[i] + op2[i];
            }
            return new Vector(result);
        }
        public static Vector operator -(Vector op1, Vector op2)
        {
            if (op1.Dimension != op2.Dimension) throw new VectorException("Dimensions of vectors in - operator are not the same.");
            double[] result = new double[op1.Dimension];
            for (sbyte i = 0; i < result.Length; i++)
            {
                result[i] = op1[i] - op2[i];
            }
            return new Vector(result);
        }
        public static Vector operator /(Vector op1, Vector op2)
        {
            if (op1.Dimension != op2.Dimension) throw new VectorException("Dimensions of vectors in - operator are not the same.");
            double[] result = new double[op1.Dimension];
            for (sbyte i = 0; i < result.Length; i++)
            {
                result[i] = op1[i] / op2[i];
            }
            return new Vector(result);
        }
        public static Vector operator *(double op1, Vector op2)
        {
            double[] result = new double[op2.Dimension];
            for (sbyte i = 0; i < result.Length; i++)
            {
                result[i] = op1 * op2[i];
            }
            return new Vector(result);
        }
        public static Vector operator *(Vector op1, double op2)
        {
            return op2 * op1;
        }
        public double Length //возвращает длину вектора (модуль, норму)
        {
            get
            {
                double sum = 0.0;
                for (sbyte i = 0; i < Dimension; i++)
                {
                    sum += components[i] * components[i];
                }
                return Math.Sqrt(sum);
            }
        }
        public override string ToString()
        {
            return String.Join(" ", components);
        }
    }
    class VectorException : Exception
    {
        public VectorException() : base("Incorrect vector.") { }
        public VectorException(string message) : base(message) { }
    }
}

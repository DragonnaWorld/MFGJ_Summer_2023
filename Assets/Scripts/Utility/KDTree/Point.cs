using System;
using System.Diagnostics;
using System.Text;

namespace KDTree
{
    public class Point<CoordinateType>
    {
        public CoordinateType[] Data;
        public readonly int Dimension;

        public Point(int size)
        {
            if (size <= 0)
                throw new ArgumentException($"Invalid point dimension: size = {size}");
            Data = new CoordinateType[size];
            Dimension = size;
        }

        public Point(CoordinateType x, CoordinateType y)
        {
            Data = new CoordinateType[] { x, y };
            Dimension = 2;
        }

        public Point(CoordinateType x, CoordinateType y, CoordinateType z)
        {
            Data = new CoordinateType[] { x, y, z };
            Dimension = 3;
        }

        public Point(CoordinateType[] data)
        {
            if (data.Length == 0)
                throw new ArgumentException("Array in point initialization is empty");
            Data = data;
            Dimension = data.Length;
        }

        public CoordinateType this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public float DistanceSquared(Point<CoordinateType> other)
        {
            if (other.Dimension != Dimension)
                throw new AggregateException($"2 points do not have the same dimensions: {Dimension} and {other.Dimension}");
            float distance = 0F;
            for (int id = 0; id < Dimension; ++id)
            {
                float x = ConvertToFloat(Data[id]);
                float xp = ConvertToFloat(other[id]);
                distance += (x - xp) * (x - xp);
            }   
            return distance;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("(");
            for (int i = 0; i < Dimension; ++i)
            {
                if (i != Dimension - 1)
                    builder.Append($"{ConvertToFloat(Data[i])},");
                else
                    builder.Append($"{ConvertToFloat(Data[i])})");
            }
            return builder.ToString();
        }

        float ConvertToFloat(CoordinateType type)
        {
            return (float)Convert.ChangeType(type, typeof(float));
        }

        public float GetFloat(int index)
        {
            return ConvertToFloat(Data[index]);
        }
    }
}

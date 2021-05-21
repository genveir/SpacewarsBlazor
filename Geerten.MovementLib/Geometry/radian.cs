using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geerten.MovementLib.Geometry
{
    public struct radian
    {
        private const double TAU = 2 * Math.PI;

        private readonly double value;

        public radian(double value)
        {
            value = value % TAU;
            if (value < 0) value += TAU;

            this.value = value;
        }

        public static radian FromDegree(double degree)
        {
            return new radian(degree / 180.0d * Math.PI);
        }

        public double toDouble()
        {
            return value;
        }

        public static implicit operator radian(double value)
        {
            return new radian(value);
        }

        public static radian operator +(radian first, radian second)
        {
            return new radian(first.value + second.value);
        }

        public static radian operator -(radian first, radian second)
        {
            return new radian(first.value - second.value);
        }

        public static radian operator *(double first, radian second)
        {
            return new radian(first * second.value);
        }

        public static radian operator *(radian first, double second)
        {
            return new radian(first.value * second);
        }

        public static radian operator /(radian first, double second)
        {
            return new radian(first.value / second);
        }

        public static bool operator <(radian first, radian second)
        {
            return first.value < second.value;
        }

        public static bool operator >(radian first, radian second)
        {
            return first.value > second.value;
        }

        public override string ToString()
        {
            return string.Format("radian: " + value / TAU);
        }
    }
}

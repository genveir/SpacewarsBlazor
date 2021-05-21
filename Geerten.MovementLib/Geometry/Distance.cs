using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geerten.MovementLib.Geometry
{
    public class Distance : IComparable
    {
        public static Distance Zero = new Distance(0);

        public double Value { get; set; }

        public Distance(double value)
        {
            Value = value;
        }

        public static implicit operator double(Distance dist)
        {
            return dist.Value;
        }

        public static Distance Calculate(ILocation locationOne, ILocation locationTwo)
        {
            long XLeg = locationOne.X - locationTwo.X;

            long YLeg = locationOne.Y - locationTwo.Y;

            if (XLeg == 0)
                if (YLeg != 0) return new Distance(Math.Abs(YLeg));
                else return new Distance(0);

            var ratio = (double)YLeg / (double)XLeg;
            var hypot = Math.Abs(XLeg) * Math.Sqrt(1 + ratio * ratio);

            return new Distance(hypot);
        }

        public static Distance operator +(Distance first, Distance second)
        {
            return new Distance(first.Value + second.Value);
        }

        public static Distance operator -(Distance first, Distance second)
        {
            return new Distance(first.Value - second.Value);
        }

        public static Distance operator *(Distance first, Distance second)
        {
            return new Distance(first.Value * second.Value);
        }

        public static Distance operator /(Distance first, Distance second)
        {
            return new Distance(first.Value / second.Value);
        }

        public override string ToString()
        {
            return string.Format("Distance: {0}", Value);
        }

        public int CompareTo(object obj)
        {
            if (obj is Distance)
            {
                var otherDistance = (Distance)obj;

                return Value.CompareTo(otherDistance.Value);
            }
            else
                throw new ArgumentException("object is not a distance");
        }
    }
}

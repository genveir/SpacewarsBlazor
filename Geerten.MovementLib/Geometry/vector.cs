using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geerten.MovementLib.Geometry
{
    public struct vector
    {
        public static vector Zero = new vector(0, 0);

        public long XOffset { get; set; }
        public long YOffset { get; set; }

        public Direction Direction { get; }
        public Distance Distance { get; }

        public vector(long XOffset, long YOffset)
        {
            this.XOffset = XOffset;
            this.YOffset = YOffset;

            var vectorLoc = new FixedLocation(XOffset, YOffset);

            this.Direction = Direction.Calculate(FixedLocation.Zero, vectorLoc);
            this.Distance = Distance.Calculate(FixedLocation.Zero, vectorLoc);
        }

        public vector(Direction direction, Distance distance)
        {
            this.Direction = direction;
            this.Distance = distance;

            var vectorLoc = new FixedLocation(FixedLocation.Zero, Direction, Distance);

            this.XOffset = vectorLoc.X;
            this.YOffset = vectorLoc.Y;
        }

        public vector(ILocation location)
        {
            this.XOffset = location.X;
            this.YOffset = location.Y;

            this.Direction = Direction.Calculate(FixedLocation.Zero, location);
            this.Distance = Distance.Calculate(FixedLocation.Zero, location);
        }

        public static vector operator +(vector first, vector second)
        {
            return new vector(first.XOffset + second.XOffset, first.YOffset + second.YOffset);
        }

        public static vector operator -(vector first, vector second)
        {
            return new vector(first.XOffset - second.XOffset, first.YOffset - second.YOffset);
        }

        public static vector operator /(vector vector, double divider)
        {
            return new vector((long)(vector.XOffset / divider), (long)(vector.YOffset / divider));
        }

        public static vector operator *(vector vector, long multiplier)
        {
            return new vector(vector.XOffset * multiplier, vector.YOffset * multiplier);
        }
        public static vector operator *(long multiplier, vector vector)
        {
            return new vector(vector.XOffset * multiplier, vector.YOffset * multiplier);
        }

        public static vector operator *(vector vector, double multiplier)
        {
            return new vector((long)(vector.XOffset * multiplier), (long)(vector.YOffset * multiplier));
        }
        public static vector operator *(double multiplier, vector vector)
        {
            return new vector((long)(vector.XOffset * multiplier), (long)(vector.YOffset * multiplier));
        }

        public static vector Calculate(ILocation from, ILocation to)
        {
            return new vector(new FixedLocation(to.X - from.X, to.Y - from.Y));
        }

        public override string ToString()
        {
            return string.Format("vector ({0}, {1})", XOffset, YOffset);
        }
    }
}
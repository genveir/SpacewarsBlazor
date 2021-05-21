using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geerten.MovementLib.Geometry
{
    public class FixedLocation : ILocation
    {
        public static FixedLocation Zero = new FixedLocation(0, 0);

        public long X { get; }
        public long Y { get; }

        public FixedLocation(long X, long Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public FixedLocation(ILocation location, Direction direction, Distance distance)
        {
            this.X = (long)(Math.Sin(direction.InRadians.toDouble()) * distance) + location.X;
            this.Y = (long)(Math.Cos(direction.InRadians.toDouble()) * distance) + location.Y;
        }

        public FixedLocation(ILocation startingPoint, vector difference)
        {
            this.X = startingPoint.X + difference.XOffset;
            this.Y = startingPoint.Y + difference.YOffset;
        }

        public override string ToString()
        {
            return string.Format("FixedLocation: ({0}, {1})", X, Y);
        }
    }
}

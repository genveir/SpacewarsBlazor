using Geerten.MovementLib.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geerten.MovementLib.Geometry
{
    public class Direction
    {
        public static Direction Zero = new Direction(0);

        private radian _inRadians;

        public radian InRadians
        {
            get { return _inRadians; }
            private set { _inRadians = value; }
        }

        public double InDegrees
        {
            get { return _inRadians.toDouble() / Math.PI * 180.0d; }
            private set { _inRadians = value / 180.0d * Math.PI; }
        }

        private Direction(radian inRadians)
        {
            this.InRadians = inRadians;
        }

        public static Direction FromRadian(radian value)
        {
            return new Direction(value);
        }

        public static Direction FromDegrees(double degrees)
        {
            return new Direction(degrees / 180.0d * Math.PI);
        }

        public static Direction FromCirclePortion(double portion)
        {
            return new Direction(portion * 2 * Math.PI);
        }

        public static implicit operator radian(Direction dir)
        {
            return dir.InRadians;
        }

        public static implicit operator Direction(radian value)
        {
            return Direction.FromRadian(value);
        }

        public static Direction Calculate(ILocation from, ILocation to)
        {
            double deltaX = to.X - from.X;
            double deltaY = to.Y - from.Y;

            return Direction.FromRadian(Math.Atan2(deltaX, deltaY));
        }

        public static Direction CalculateBearing(IBodyWithHeading fromHeading, ILocation to)
        {
            var heading = Calculate(fromHeading.Location, to);

            return heading - fromHeading.Heading;
        }

        public static Direction operator +(Direction directionOne, Direction directionTwo)
        {
            return Direction.FromRadian(directionOne.InRadians + directionTwo.InRadians);
        }

        public static Direction operator -(Direction directionOne, Direction directionTwo)
        {
            return Direction.FromRadian(directionOne.InRadians - directionTwo.InRadians);
        }

        public override string ToString()
        {
            return string.Format("Direction: {0}", InRadians);
        }
    }
}

using Geerten.Movement.Geometry;
using Geerten.Movement.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacewarsBlazor.Game
{
    public class WrappingLocation : ILocation
    {
        private long x;
        private long y;

        private static long MaxX = Game.MaxX * 100;
        private static long MaxY = Game.MaxY * 100;

        public long X
        {
            get => x;
            set => x = Modulate(value, MaxX);
        }
        public long Y
        {
            get => y;
            set => y = Modulate(value, MaxY);
        }

        public WrappingLocation(long X, long Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public WrappingLocation(ILocation startingLocation, Direction direction, Distance distance)
        {
            this.X = (long)(Math.Sin(direction.InRadians.toDouble()) * distance.Value) + startingLocation.X;
            this.Y = (long)(Math.Cos(direction.InRadians.toDouble()) * distance.Value) + startingLocation.Y;
        }

        public WrappingLocation(ILocation startingLocation, vector difference)
        {
            this.X = startingLocation.X + difference.XOffset;
            this.Y = startingLocation.Y + difference.YOffset;
        }

        private static long Modulate(long input, long max)
        {
            return ((input % max) + max) % max;
        }
    }
}

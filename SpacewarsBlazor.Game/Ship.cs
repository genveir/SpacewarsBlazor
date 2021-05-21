using Geerten.MovementLib.Geometry;
using Geerten.MovementLib.Movement;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacewarsBlazor.Game
{
    class Ship : IBodyWithHeading, IMovingBody
    {
        public Ship(WrappingLocation location, Color color)
        {
            this.Location = location;
            this.Heading = Direction.FromDegrees(0);
            this.Movement = vector.Zero;
            this.Color = color;
            this.Size = 1000;
        }

        public void Update(ShipCommands commands)
        {
            var newLocation = new WrappingLocation(Location, Movement);

            if (commands.TurnLeft) Heading = Heading - Direction.FromDegrees(5);
            if (commands.TurnRight) Heading = Heading + Direction.FromDegrees(5);
            if (commands.Accelerate) Movement = Movement + new vector(Heading, new Distance(5.0d));
            if (commands.Fire)
            {
                Fire();
                commands.Fire = false;
            }

            this.Location = newLocation;
        }

        private void Fire()
        {
            var rightInFront = new WrappingLocation(this.Location, Heading, new Distance(5000.0d));
            var fireVector = new vector(Heading, new Distance(500.0d));
            var bulletVector = this.Movement + fireVector;

            var bullet = new Bullet(rightInFront, bulletVector);

            Game.Subscribe(bullet);
        }

        public vector Movement { get; private set; }

        public Direction Heading { get; private set; }

        public ILocation Location { get; private set; }

        public long Size { get; private set; }

        public Color Color { get; private set; }
    }

    public class ShipCommands
    {
        public static ShipCommands None => new ShipCommands();

        public bool TurnLeft;
        public bool TurnRight;
        public bool Accelerate;
        public bool Fire;
    }
}

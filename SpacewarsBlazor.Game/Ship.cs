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
        private ShipParameters ShipParameters { get; set; }

        public Ship(WrappingLocation location, Color color, ShipParameters parameters = null)
        {
            this.ShipParameters = parameters ?? new ShipParameters();

            this.Location = location;
            this.Heading = Direction.FromDegrees(0);
            this.Movement = vector.Zero;
            this.Color = color;
            this.Size = ShipParameters.Size;
            this.Energy = ShipParameters.Energy;
        }

        public void Update(ShipCommands commands)
        {
            var newLocation = new WrappingLocation(Location, Movement);

            if (commands.TurnLeft) Heading = Heading - Direction.FromRadian(ShipParameters.RotationSpeed);
            if (commands.TurnRight) Heading = Heading + Direction.FromRadian(ShipParameters.RotationSpeed);
            if (commands.Accelerate) Movement = Movement + new vector(Heading, new Distance(ShipParameters.AccelerationSpeed));
            if (commands.Fire)
            {
                Fire();
                commands.Fire = false;
            }

            this.Energy += ShipParameters.EnergyRegen;
            if (Energy > ShipParameters.Energy) Energy = ShipParameters.Energy;
            this.Location = newLocation;
        }

        private void Fire()
        {
            if (this.Energy > ShipParameters.MinEnergyToFire)
            {
                this.Energy -= ShipParameters.EnergyToFire;

                var orthogonalDirection = Direction.FromRadian(Heading.InRadians + radian.FromDegree(-90));

                var rightInFront = new WrappingLocation(this.Location, Heading, new Distance(2000.0d));
                var bitLeft = new WrappingLocation(rightInFront, orthogonalDirection, new Distance(1000.0d));
                var bitRight = new WrappingLocation(rightInFront, orthogonalDirection, new Distance(-1000.0d));

                var fireVector = new vector(Heading, new Distance(ShipParameters.BulletVelocity));
                var bulletVector = this.Movement + fireVector;

                var bulletLeft = new Bullet(bitLeft, bulletVector, ShipParameters.BulletSize, ShipParameters.BulletDamage);
                var bulletRight = new Bullet(bitRight, bulletVector, ShipParameters.BulletSize, ShipParameters.BulletDamage);
                bulletLeft.Update();
                bulletRight.Update();

                Game.Subscribe(bulletLeft);
                Game.Subscribe(bulletRight);
            }
        }

        public void Hit(Bullet bullet)
        {
            this.Energy -= bullet.Damage;
            if (this.Energy < -500) this.Energy = -500;
        }

        public vector Movement { get; private set; }

        public Direction Heading { get; private set; }

        public ILocation Location { get; private set; }

        public long Size { get; private set; }

        public long Energy { get; private set; }

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

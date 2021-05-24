using Geerten.Movement.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacewarsBlazor.Game
{
    public class ShipParameters
    {
        protected ShipParameters() { }

        public static ShipParameters Default()
        {
            return new ShipParameters()
            {
                Size = 1500,
                Energy = 1000,
                EnergyRegen = 3,
                RotationSpeed = radian.FromDegree(5),
                AccelerationSpeed = new Distance(6.0),
                MaximumSpeed = new Distance(700),
                MinEnergyToFire = 100,
                EnergyToFire = 30,
                BulletVelocity = new Distance(500.0d),
                BulletSize = 2,
                BulletDamage = 500,
                BulletLifetime = TimeSpan.FromMilliseconds(3000)
            };
        }

        public long Size { get; private set; }

        public long Energy { get; private set; }
        public long EnergyRegen { get; private set; }

        public radian RotationSpeed { get; private set; }
        public Distance AccelerationSpeed { get; private set; }
        public Distance MaximumSpeed { get; private set; }

        public long MinEnergyToFire { get; private set; }
        public long EnergyToFire { get; private set; }

        public Distance BulletVelocity { get; private set; }
        public long BulletSize { get; private set; }
        public long BulletDamage { get; private set; }
        public TimeSpan BulletLifetime { get; private set; }
    }
}

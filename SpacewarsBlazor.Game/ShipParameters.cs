using Geerten.MovementLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacewarsBlazor.Game
{
    public class ShipParameters
    {
        public long Size { get; set; } = 1500;

        public long Energy { get; set; } = 1000;
        public long EnergyRegen { get; set; } = 3;

        public radian RotationSpeed { get; set; } = radian.FromDegree(5);
        public Distance AccelerationSpeed { get; set; } = new Distance(6.0d);

        public long MinEnergyToFire { get; set; } = 100;
        public long EnergyToFire { get; set; } = 20;
        public Distance BulletVelocity { get; set; } = new Distance(500.0d);
        public long BulletSize { get; set; } = 2;
        public long BulletDamage { get; set; } = 500;
    }
}

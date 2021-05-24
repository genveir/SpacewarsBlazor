using Geerten.Movement.Geometry;
using Geerten.Movement.Bodies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geerten.Movement.Location;

namespace SpacewarsBlazor.Game
{
    public class Bullet : BodyBase, IWithTimeout, IUpdatable
    {
        private static long bulletIdCounter = 0;

        private DateTime timeOutMoment;

        public long X => Game.MaxX - ((int)Location.X / 100) - 1; // canvas is flipped to grid
        public long Y => (int)Location.Y / 100;

        public string Color = ColorTranslator.ToHtml(System.Drawing.Color.OrangeRed);

        public long Size { get; private set; }

        public long Damage { get; private set; }

        public Bullet(ILocation location, vector vector, long size, long damage, TimeSpan timeout) : base(location, vector)
        {
            this.Id = bulletIdCounter++;
            this.Size = size;
            this.Damage = damage;
            this.timeOutMoment = DateTime.Now.Add(timeout);
        }

        public long Id { get; }

        public bool IsTimedOut => timeOutMoment < DateTime.Now;

        public bool HasHit;

        public void Update()
        {
            this.Location = new WrappingLocation(Location, Movement);
        }

        public override string ToString()
        {
            return $"bullet {Id} at {X}, {Y} with vector {Movement}";
        }
    }
}

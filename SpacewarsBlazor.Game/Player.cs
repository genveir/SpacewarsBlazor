using Geerten.Movement.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacewarsBlazor.Engine
{
    public class Player : IWithTimeout, IUpdatable
    {
        private static int playerIdCounter = 0;
        private static Random random = new Random();

        public bool Inactive = false;
        public bool CurrentlyDead => ship.Energy < 0;

        public long Id { get; }

        public long X => Game.MaxX - ((int)ship.Location.X / 100) - 1; // canvas is flipped to grid
        public long Y => (int)ship.Location.Y / 100;
        public float Heading => (float)ship.Heading.InRadians.toDouble();
        public string Color => ColorTranslator.ToHtml(ship.Color);
        public double Size => ship.Size / 100;

        public long Energy => ship.Energy;

        public ShipCommands Commands { get; }

        private Ship ship { get; }

        public Player()
        {
            Id = playerIdCounter++;

            Commands = new ShipCommands();

            ship = new Ship(
                new WrappingLocation(random.Next(Game.MaxX * 100), random.Next(Game.MaxY * 100)),
                System.Drawing.Color.FromArgb(128 + random.Next(127), 128 + random.Next(127), 128 + random.Next(127)));
        }

        public void Update()
        {
            if (!this.CurrentlyDead)
            {
                ship.Update(Commands);
            }
            else
            {
                ship.Update(ShipCommands.None);
            }
        }

        public void Hit(Bullet bullet)
        {
            ship.Hit(bullet);
        }

        private DateTime LastActive { get; set; } = DateTime.Now;
        public void Touch()
        {
            this.LastActive = DateTime.Now;
        }

        public bool IsTimedOut => LastActive.AddSeconds(5) < DateTime.Now;
    }
}

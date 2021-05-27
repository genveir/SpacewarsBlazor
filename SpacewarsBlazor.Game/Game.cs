using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SpacewarsBlazor.Engine
{
    public static class Game
    {
        private static object lockObj = new object();

        public static int MaxX = 1200;
        public static int MaxY = 900;

        private static Dictionary<long, Player> _players = new Dictionary<long, Player>();
        private static Dictionary<long, Bullet> _bullets = new Dictionary<long, Bullet>();

        public static Player[] PlayerSnapshot { get; private set; } = new Player[0];
        public static Bullet[] BulletSnapshot { get; private set; } = new Bullet[0];

        private static void SetSnapshots()
        {
            lock (lockObj)
            {
                PlayerSnapshot = _players
                    .Select(p => p.Value)
                    .ToArray();

                BulletSnapshot = _bullets
                    .Select(b => b.Value)
                    .ToArray();
            }
        }

        public static void Subscribe(Player player)
        {
            lock (lockObj)
            {
                _players.Add(player.Id, player);
            }
        }

        public static void Subscribe(Bullet bullet)
        {
            lock (lockObj)
            {
                _bullets.Add(bullet.Id, bullet);
            }
        }

        public static void UpdateLoop() 
        {
            while (true)
            {
                try
                {
                    var next = DateTime.Now.AddMilliseconds(1000 / 120); // 120 fps, fast enough to have new frames each time a client renders

                    Update();

                    var delay = next - DateTime.Now;
                    if (delay < TimeSpan.Zero) delay = TimeSpan.Zero;

                    Thread.Sleep(delay);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void Update()
        {
            var allShips = PlayerSnapshot;
            var allBullets = BulletSnapshot;

            var collisions = Collision.ResolveCollisions(allShips, allBullets);

            foreach (var collision in collisions)
            {
                if (collision.CollisionType == CollisionType.Bullet)
                {
                    var bullet = _bullets[collision.Bullet.Id];
                    bullet.HasHit = true;

                    var player = _players[collision.Collider.Id];
                    player.Hit(bullet);
                }
            }

            UpdatePlayers(allShips);
            UpdateBullets(allBullets);

            SetSnapshots();
        }

        private static void UpdatePlayers(Player[] players)
        { 
            List<Player> toRemove = new List<Player>();

            foreach (var player in players)
            {
                if (player.IsTimedOut)
                {
                    toRemove.Add(player);
                    continue;
                }

                player.Update();
            }

            lock (lockObj)
            {
                foreach (var player in toRemove)
                {
                    player.Inactive = true;
                    _players.Remove(player.Id, out _);
                }
            }
        }

        private static void UpdateBullets(Bullet[] bullets)
        {
            List<Bullet> toRemove = new List<Bullet>();

            foreach(var bullet in bullets)
            {
                if (bullet.HasHit || bullet.IsTimedOut)
                {
                    toRemove.Add(bullet);
                    continue;
                }

                bullet.Update();
            }

            lock (lockObj)
            {
                foreach (var bullet in toRemove)
                {
                    _bullets.Remove(bullet.Id, out _);
                }
            }
        }
    }
}

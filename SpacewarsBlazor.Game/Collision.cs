using Geerten.Movement.Geometry;
using Geerten.Movement.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacewarsBlazor.Game
{
    public static class Collision
    {
        private static CollisionBucket[][] buckets;
        private static CollisionBucket[] allBuckets;

        static Collision()
        {
            var bucketList = new List<CollisionBucket>();

            var maxX = (Game.MaxX / 100);
            var maxY = (Game.MaxY / 100);

            buckets = new CollisionBucket[maxX][];
            for (int x = 0; x < maxX; x++)
            {
                buckets[x] = new CollisionBucket[maxY];
                for (int y = 0; y < maxY; y++)
                {
                    buckets[x][y] = new CollisionBucket();
                    bucketList.Add(buckets[x][y]);
                }
            }

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    var xMin1 = (x - 1 + maxX) % maxX;
                    var xPlus1 = (x + 1 + maxX) % maxX;
                    var yMin1 = (y - 1 + maxY) % maxY;
                    var yPlus1 = (y + 1 + maxY) % maxY;

                    //top
                    buckets[x][y].LinkNeighbour(buckets[x][yMin1]);

                    //left
                    buckets[x][y].LinkNeighbour(buckets[xMin1][y]);

                    //topright
                    buckets[x][y].LinkNeighbour(buckets[xPlus1][yMin1]);

                    //topleft
                    buckets[x][y].LinkNeighbour(buckets[xMin1][yMin1]);
                }
            }

            allBuckets = bucketList.ToArray();
        }

        public static IEnumerable<CollisionPair> ResolveCollisions(IEnumerable<Player> allPlayers, IEnumerable<Bullet> allBullets)
        {
            foreach (var bucket in allBuckets) bucket.Empty();

            var players = allPlayers.Select(p => new CollisionPlayer() { Id = p.Id, Size = p.Size, location = new FixedLocation(p.X, p.Y) });
            var bullets = allBullets.Select(b => new CollisionBullet() { Id = b.Id, Size = b.Size, location = new FixedLocation(b.X, b.Y) });

            foreach (var co in players) buckets[co.location.X / 100][co.location.Y / 100].Add(co);
            foreach (var co in bullets) buckets[co.location.X / 100][co.location.Y / 100].Add(co);

            var result = new HashSet<CollisionPair>();
            foreach (var bucket in allBuckets) bucket.ResolveCollisions(result);

            return result;
        }
    }

    public class CollisionBucket
    {
        private List<CollisionPlayer> players;
        private List<CollisionBullet> bullets;

        private List<CollisionBucket> neighbours;

        private bool Handled;

        public CollisionBucket()
        {
            this.neighbours = new List<CollisionBucket>();
            this.players = new List<CollisionPlayer>();
            this.bullets = new List<CollisionBullet>();
        }

        public void LinkNeighbour(CollisionBucket bucket, bool backLink = true)
        {
            neighbours.Add(bucket);
            if (backLink) bucket.LinkNeighbour(this, false);
        }

        public void Empty()
        {   
            players.Clear();
            bullets.Clear();

            Handled = false;
        }

        public void Add(CollisionPlayer collisionPlayer)
        {
            players.Add(collisionPlayer);
        }

        public void Add(CollisionBullet collisionBullet)
        {
            bullets.Add(collisionBullet);
        }

        public void ResolveCollisions(HashSet<CollisionPair> result)
        {
            var relevantBuckets = neighbours.Where(n => !n.Handled).Append(this);

            var relevantPlayers = relevantBuckets.SelectMany(rb => rb.players);
            var relevantBullets = relevantBuckets.SelectMany(rb => rb.bullets);

            foreach (var player in relevantPlayers) player.Handled = false;

            foreach(var player in relevantPlayers)
            {
                player.Handled = true;

                // check against all bullets
                foreach (var bullet in relevantBullets)
                {
                    var collides = CheckCollision(player, bullet);

                    if (collides) result.Add(new CollisionPair(player, bullet));
                }

                // check against other players that haven't already checked if they collide with you
                foreach (var otherPlayer in relevantPlayers)
                {
                    if (otherPlayer.Handled) continue;

                    var collides = CheckCollision(player, otherPlayer);

                    if (collides) result.Add(new CollisionPair(player, otherPlayer));
                }
            }

            Handled = true;
        }

        private bool CheckCollision(CollisionObject first, CollisionObject second)
        {
            var distance = Distance.Calculate(first.location, second.location);
            var allowedDistance = (first.Size + second.Size);
            var collides = distance.Value < allowedDistance;

            return collides;
        }
    }

    public class CollisionPlayer : CollisionObject { public bool Handled; }
    public class CollisionBullet : CollisionObject { }

    public class CollisionObject
    {
        public long Id;
        public FixedLocation location;
        public double Size;

        public bool HasBulletCollision;
    }

    public enum CollisionType { Player, Bullet }

    public class CollisionPair
    {
        public CollisionPair(CollisionPlayer collider, CollisionPlayer otherPlayer)
        {
            CollisionType = CollisionType.Player;
            Collider = collider;
            OtherPlayer = otherPlayer;
        }

        public CollisionPair(CollisionPlayer collider, CollisionBullet bullet)
        {
            CollisionType = CollisionType.Bullet;
            Collider = collider;
            Bullet = bullet;
        }

        public CollisionType CollisionType { get; }

        public CollisionPlayer Collider { get; }

        public CollisionPlayer OtherPlayer { get; }
        public CollisionBullet Bullet { get; }

        public override string ToString()
        {
            switch(CollisionType)
            {
                case CollisionType.Player: return $"collision between players {Collider.Id} and {OtherPlayer.Id}";
                case CollisionType.Bullet: return $"collision between player {Collider.Id} and bullet {Bullet.Id}";
                default: return "faulty collision";
            }
        }

        public override int GetHashCode()
        {
            return (int)(Collider.Id + (OtherPlayer?.Id ?? 0) + (Bullet?.Id ?? 0));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            
            var other = obj as CollisionPair;
            if (other == null) return false;
            return
                other.CollisionType == this.CollisionType &&
                other.Collider.Id == this.Collider.Id &&
                other.OtherPlayer?.Id == this.OtherPlayer?.Id &&
                other.Bullet?.Id == this.Bullet?.Id;
        }
    }
}

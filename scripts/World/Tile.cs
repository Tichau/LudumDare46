using Godot;
using System.Collections.Generic;

namespace World
{
    public struct TileCoordinates
    {
        public static Vector2 CellSize = new Vector2(32, 32);

        public int X;
        public int Y;

        public TileCoordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static TileCoordinates FromPosition(Vector2 position)
        {
            return new TileCoordinates
            {
                X = (int)position.x / (int)TileCoordinates.CellSize.x,
                Y = (int)position.y / (int)TileCoordinates.CellSize.y,
            };
        }

        public override string ToString()
        {
            return $"{this.X},{this.Y}";
        }

        /// <summary>
        /// Distance from another tile in manhattan distance.
        /// </summary>
        /// <param name="other">The other tile.</param>
        /// <returns>Returns the distance from the other tile.</returns>
        public int DistanceFrom(TileCoordinates other)
        {
            return Mathf.Abs(this.X - other.X) + Mathf.Abs(this.Y - other.Y);
        }

        /// <Summary>
        /// Returns the coordinates of the 8 tiles around (in trigonometric order). 
        /// </Summary>
        public IEnumerable<TileCoordinates> GetNeighbours()
        {
            yield return new TileCoordinates(this.X + 1, this.Y    );
            yield return new TileCoordinates(this.X + 1, this.Y - 1);
            yield return new TileCoordinates(this.X    , this.Y - 1);
            yield return new TileCoordinates(this.X - 1, this.Y - 1);
            yield return new TileCoordinates(this.X - 1, this.Y    );
            yield return new TileCoordinates(this.X - 1, this.Y + 1);
            yield return new TileCoordinates(this.X    , this.Y + 1);
            yield return new TileCoordinates(this.X + 1, this.Y + 1);
        }

        /// <Summary>
        /// Returns a sorted list of tile coordinates in radius (using manhattan distance).
        /// </Summary>
        /// <param name="radius">Radius of the area (manhattan distance).</param>
        public IEnumerable<TileCoordinates> GetArea(int radius)
        {
            yield return new TileCoordinates(this.X, this.Y);

            for (int r = 1; r <= radius; r++)
            {
                for (int i = 1; i <= r; i++)
                {
                    int a = i;
                    int b = r - i;

                    if (b > a) 
                    {
                        continue;
                    }

                    yield return new TileCoordinates(this.X + a, this.Y + b);
                    yield return new TileCoordinates(this.X - a, this.Y + b);

                    if (b > 0)
                    {
                        yield return new TileCoordinates(this.X + a, this.Y - b);
                        yield return new TileCoordinates(this.X - a, this.Y - b);
                    }

                    if (a != b)
                    {
                        yield return new TileCoordinates(this.X + b, this.Y + a);
                        yield return new TileCoordinates(this.X + b, this.Y - a);
                        
                        if (b > 0)
                        {
                            yield return new TileCoordinates(this.X - b, this.Y + a);
                            yield return new TileCoordinates(this.X - b, this.Y - a);
                        }
                    }
                }
            }
        }
    }

    public enum TileType : int
    {
        Grass,
        Water,
    }
}

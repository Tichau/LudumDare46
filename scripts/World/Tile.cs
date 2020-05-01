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

        public IEnumerable<TileCoordinates> GetNeighbours()
        {
            // In trigonometric order.
            yield return new TileCoordinates(this.X + 1, this.Y    );
            yield return new TileCoordinates(this.X + 1, this.Y - 1);
            yield return new TileCoordinates(this.X    , this.Y - 1);
            yield return new TileCoordinates(this.X - 1, this.Y - 1);
            yield return new TileCoordinates(this.X - 1, this.Y    );
            yield return new TileCoordinates(this.X - 1, this.Y + 1);
            yield return new TileCoordinates(this.X    , this.Y + 1);
            yield return new TileCoordinates(this.X + 1, this.Y + 1);
        }
    }

    public enum TileType : int
    {
        Grass,
        Water,
    }
}

using Godot;
using System.Collections.Generic;

namespace World
{
    public struct TileCoordinates
    {
        public int X;
        public int Y;

        public TileCoordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static TileCoordinates FromPosition(Vector2 position, Vector2 cellSize)
        {
            return new TileCoordinates
            {
                X = (int)position.x / (int)cellSize.x,
                Y = (int)position.y / (int)cellSize.y,
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

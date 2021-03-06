using Godot;
using System.Collections.Generic;

namespace World
{
    public struct PatchCoordinates
    {
        public const int PatchWidth = 32;
        public const int PatchHeight = 32;

        public int X;
        public int Y;

        public PatchCoordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Rect2 TileRect => new Rect2(new Vector2(this.X * PatchCoordinates.PatchWidth, this.Y * PatchCoordinates.PatchHeight), PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight);

        public static PatchCoordinates FromTile(TileCoordinates tileCoordinates)
        {
            var coordinates = new PatchCoordinates()
            {
                X = tileCoordinates.X / PatchWidth,
                Y = tileCoordinates.Y / PatchHeight
            };

            if (tileCoordinates.X < 0)
            {
                coordinates.X--;
            }

            if (tileCoordinates.Y < 0)
            {
                coordinates.Y--;
            }

            return coordinates;
        }

        public override string ToString()
        {
            return $"{this.X},{this.Y}";
        }

        public IEnumerable<PatchCoordinates> GetNeighbours()
        {
            // In trigonometric order.
            yield return new PatchCoordinates(this.X + 1, this.Y    );
            yield return new PatchCoordinates(this.X + 1, this.Y - 1);
            yield return new PatchCoordinates(this.X    , this.Y - 1);
            yield return new PatchCoordinates(this.X - 1, this.Y - 1);
            yield return new PatchCoordinates(this.X - 1, this.Y    );
            yield return new PatchCoordinates(this.X - 1, this.Y + 1);
            yield return new PatchCoordinates(this.X    , this.Y + 1);
            yield return new PatchCoordinates(this.X + 1, this.Y + 1);
        }
    }
}

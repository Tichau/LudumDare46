using Godot;

namespace World
{
    public struct PatchCoordinates
    {
        public const int PatchWidth = 128;
        public const int PatchHeight = 128;

        public int X;
        public int Y;

        public PatchCoordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Rect2 TileRect => new Rect2(new Vector2(this.X * PatchCoordinates.PatchWidth, this.Y * PatchCoordinates.PatchHeight), PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight);
    }
}

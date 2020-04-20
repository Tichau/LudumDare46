using Godot;

public class WorldMap : TileMap
{
    private float _debugScale = 1f;

    private TileInfo[] tileInfoByTile;

    private World.Generator worldGenerator;

    public override void _Ready()
    {
        // Load tile indexes.
        string[] tileNames = System.Enum.GetNames(typeof(World.Tile));
        this.tileInfoByTile = new TileInfo[tileNames.Length];
        for (int index = 0; index < this.tileInfoByTile.Length; index++)
        {
            ref TileInfo tile = ref this.tileInfoByTile[index];

            tile.Tile = (World.Tile)index;
            tile.TileIndex = this.TileSet.FindTileByName(tileNames[index]);
            if (tile.TileIndex < 0)
            {
                GD.Print($"Can't retrieve tile index for tile {tileNames[index]}");
            }
        }

        World.Generator.Settings settings = new World.Generator.Settings
        {
            Seed = 1,
            SeaLevel = 0.5f,
            ChaosLevel = 0.6f,
        };

        this.worldGenerator = new World.Generator(settings);
        
        this.GenerateWorldPatch(new World.PatchCoordinates(-1, 0));
        this.GenerateWorldPatch(new World.PatchCoordinates(0, 0));
        this.GenerateWorldPatch(new World.PatchCoordinates(-1, -1));
        this.GenerateWorldPatch(new World.PatchCoordinates(0, -1));
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("debug_dezoom"))
        {
            this._debugScale /= 2;

            (this.GetParent() as Node2D).Scale = new Vector2(this._debugScale, this._debugScale);
        }
        else if (Input.IsActionJustReleased("debug_zoom"))
        {
            this._debugScale *= 2;
            this._debugScale = Mathf.Clamp(this._debugScale, 0f, 1f);

            (this.GetParent() as Node2D).Scale = new Vector2(this._debugScale, this._debugScale);
        }
    }

    private void GenerateWorldPatch(World.PatchCoordinates patchCoordinates)
    {
        var worldPatch = this.worldGenerator.GenerateWorldPatch(patchCoordinates);

        // Apply generated world patch to tilemap.
        var rect = patchCoordinates.TileRect;
        for (int i = 0; i < (int)rect.Size.x; i++)
        {
            for (int j = 0; j < (int)rect.Size.y; j++)
            {
                ref var tileInfo = ref this.tileInfoByTile[(int)worldPatch[i, j]];
                this.SetCell((int)rect.Position.x + i, (int)rect.Position.y + j, tileInfo.TileIndex);
            }
        }

        this.UpdateBitmaskRegion(rect.Position, rect.End);
    }

    private struct TileInfo
    {
        public World.Tile Tile;
        public int TileIndex;
    }
}

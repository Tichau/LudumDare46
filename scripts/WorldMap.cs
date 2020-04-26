using Godot;

public class WorldMap : TileMap
{
    [Signal]
    public delegate void GeneratorSettingsChanged();

    private PlayerController player;

    private TileInfo[] tileInfoByTile;

    private World.Generator worldGenerator;
    private System.Collections.Generic.HashSet<World.PatchCoordinates> patchLoaded = new System.Collections.Generic.HashSet<World.PatchCoordinates>();

    private float _debugScale = 1f;

    public World.GeneratorSettings GeneratorSettings
    {
        get => this.worldGenerator.Settings;
        set
        {
            this.worldGenerator.ChangeSettings(value);

            this.patchLoaded.Clear();

            this.EmitSignal("GeneratorSettingsChanged");
        }
    }

    public override void _Ready()
    {
        // Load tile indexes.
        string[] tileNames = System.Enum.GetNames(typeof(World.TileType));
        this.tileInfoByTile = new TileInfo[tileNames.Length];
        for (int index = 0; index < this.tileInfoByTile.Length; index++)
        {
            ref TileInfo tile = ref this.tileInfoByTile[index];

            tile.Tile = (World.TileType)index;
            tile.TileIndex = this.TileSet.FindTileByName(tileNames[index]);
            if (tile.TileIndex < 0)
            {
                GD.Print($"Can't retrieve tile index for tile {tileNames[index]}");
            }
        }

        // Get player
        this.player = this.GetNode("/root/Game/Player") as PlayerController;
        Debug.Assert(this.player);

        var settings = new World.GeneratorSettings
        {
            Seed = 2,
            SeaLevel = 0.45f,
            ChaosLevel = 0.7f,
        };

        this.worldGenerator = new World.Generator(settings);

        this.EmitSignal("GeneratorSettingsChanged");

        int preloadPatchCount = 0;
        for (int i = -preloadPatchCount; i < preloadPatchCount; i++)
        {
            for (int j = -preloadPatchCount; j < preloadPatchCount; j++)
            {
                this.GenerateWorldPatch(new World.PatchCoordinates(i, j));
            }
        }
    }

    public override void _Process(float delta)
    {
        var playerTile = World.TileCoordinates.FromPosition(this.player.Position, this.CellSize);
        World.PatchCoordinates currentPatch = World.PatchCoordinates.FromTile(playerTile);
        
        if (!this.patchLoaded.Contains(currentPatch))
        {
            this.GenerateWorldPatch(currentPatch);
        }

        foreach (var neighbour in currentPatch.GetNeighbours())
        {
            if (!this.patchLoaded.Contains(neighbour))
            {
                this.GenerateWorldPatch(neighbour);
            }
        }

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

        this.patchLoaded.Add(patchCoordinates);

        Debug.Log($"Patch {patchCoordinates} generated.");
    }

    private struct TileInfo
    {
        public World.TileType Tile;
        public int TileIndex;
    }
}

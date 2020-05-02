using Godot;

public class AmbienceController : Node2D
{
    [Export]
    private int waterAttenuationDistanceStart = 5;

    [Export]
    private int waterAttenuationDistanceEnd = 20;
    
    [Export]
    private float grassDuckingDb = -9f;

    /// <summary>
    /// Attenuation speed in dB per second.
    /// </summary>
    [Export]
    private float attenuationSpeed = 9;
    
    private float waterVolumeDbTarget = 0;
    private float grassVolumeDbTarget = 0;

    private AudioStreamPlayer grassAmbience;
    private AudioStreamPlayer waterAmbience;
    private WorldMap worldMap;

    public override void _Ready()
    {
        this.worldMap = this.GetNode("/root/Game/World") as WorldMap;
        Debug.Assert(this.worldMap);

        this.grassAmbience = this.GetNode("Grass") as AudioStreamPlayer;
        Debug.Assert(this.grassAmbience);
        this.grassAmbience.Playing = true;

        this.waterAmbience = this.GetNode("Water") as AudioStreamPlayer;
        Debug.Assert(this.waterAmbience);
        this.waterAmbience.Playing = true;

        // Initialize volumes.
        this.waterVolumeDbTarget = this.ComputeWaterVolumeDbTarget();
        this.waterAmbience.VolumeDb = this.waterVolumeDbTarget;
        this.grassVolumeDbTarget = this.ComputeGrassVolumeDbTarget(this.waterVolumeDbTarget);
        this.grassAmbience.VolumeDb = this.grassVolumeDbTarget;
    }

    public override void _Process(float delta)
    {
        this.waterVolumeDbTarget = this.ComputeWaterVolumeDbTarget();
        this.grassVolumeDbTarget = this.ComputeGrassVolumeDbTarget(this.waterVolumeDbTarget);

        this.waterAmbience.VolumeDb += Mathf.Clamp(this.waterVolumeDbTarget - this.waterAmbience.VolumeDb, -this.attenuationSpeed * delta, this.attenuationSpeed * delta);
        this.grassAmbience.VolumeDb += Mathf.Clamp(this.grassVolumeDbTarget - this.grassAmbience.VolumeDb, -this.attenuationSpeed * delta, this.attenuationSpeed * delta);
        // Debug.Log($"Grass {this.grassAmbience.VolumeDb}dB -> {this.grassVolumeDbTarget}dB Water {this.waterAmbience.VolumeDb}dB -> {this.waterVolumeDbTarget}dB");
    }
    
    public float ComputeWaterVolumeDbTarget()
    {
        var currentTile = World.TileCoordinates.FromPosition(this.GlobalPosition);

        int distanceFromWater = int.MaxValue;
        foreach (var tile in currentTile.GetArea(this.waterAttenuationDistanceEnd))
        {
            var tileType = (World.TileType)this.worldMap.GetCell(tile.X, tile.Y);
            switch (tileType)
            {
                case World.TileType.Water:
                    distanceFromWater = System.Math.Min(distanceFromWater, currentTile.DistanceFrom(tile));
                    break;
            }

            if (distanceFromWater < int.MaxValue)
            {
                break;
            }
        }

        return Mathf.Clamp((distanceFromWater - this.waterAttenuationDistanceStart) / (float)(this.waterAttenuationDistanceEnd - this.waterAttenuationDistanceStart), 0, 1) * -60f;
    }

    public float ComputeGrassVolumeDbTarget(float waterVolumeDb)
    {
        return this.grassDuckingDb - Mathf.Clamp(waterVolumeDb, this.grassDuckingDb, 0f);
    }
}

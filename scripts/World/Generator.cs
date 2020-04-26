using System;

namespace World
{
    public class Generator
    {
        private const int NoiseOctaveCount = 5;
        
        private TileType[,] patchCache;
        private TileType[,] patchOperationCache;
        private float[][,] noiseByOctave;
        private float[,] floatCache;

        public Generator(GeneratorSettings settings)
        {
            // Instantiate data structures.
            this.patchCache = new TileType[PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight];
            this.patchOperationCache = new TileType[PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight];
            this.floatCache = new float[PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight];
            this.noiseByOctave = new float[NoiseOctaveCount][,];
            
            this.ChangeSettings(settings);
        }
        
        public GeneratorSettings Settings
        {
            get;
            private set;
        }

        public void ChangeSettings(GeneratorSettings settings)
        {
            this.Settings = settings;

            Random random = new Random(this.Settings.Seed);
            this.noiseByOctave[0] = Noise.GenerateWhiteNoise(PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight, random);
        }

        public TileType[,] GenerateWorldPatch(PatchCoordinates patchCoordinates)
        {
            Debug.Assert(this.patchCache != null && this.patchCache.GetLength(0) == PatchCoordinates.PatchWidth && this.patchCache.GetLength(1) == PatchCoordinates.PatchHeight, "Invalid world patch");

            System.Array.Clear(this.patchCache, 0, PatchCoordinates.PatchWidth * PatchCoordinates.PatchHeight);

            Random random = new Random(this.Settings.Seed.GetHashCode() + patchCoordinates.GetHashCode());

            // Generate terrain type.
            for (int octave = 1; octave < NoiseOctaveCount; octave++)
            {
                Noise.GenerateStretchNoise(this.noiseByOctave[0], patchCoordinates.X, patchCoordinates.Y, octave, Noise.LinearInterpolation, ref this.noiseByOctave[octave]);
            }

            float perlinPersistance = System.Math.Max(this.Settings.ChaosLevel, 0.01f);
            Noise.GeneratePerlinNoise(this.noiseByOctave, perlinPersistance, ref this.floatCache);

            for (int i = 0; i < PatchCoordinates.PatchWidth; i++)
            {
                for (int j = 0; j < PatchCoordinates.PatchHeight; j++)
                {
                    if (this.floatCache[i, j] > this.Settings.SeaLevel)
                    {
                        this.patchCache[i, j] = TileType.Grass;
                    }
                    else
                    {
                        this.patchCache[i, j] = TileType.Water;
                    }
                }
            }
            
            // Grow water tiles to avoid invalid patterns.
            System.Array.Clear(this.patchOperationCache, 0, PatchCoordinates.PatchWidth * PatchCoordinates.PatchHeight);
            for (int i = 0; i < PatchCoordinates.PatchWidth; i++)
            {
                for (int j = 0; j < PatchCoordinates.PatchHeight; j++)
                {
                    if (this.patchCache[i, j] == TileType.Water)
                    {
                        var tile = new World.TileCoordinates(i, j);
                        foreach (var neighbour in tile.GetNeighbours())
                        {
                            if (neighbour.X < 0 || neighbour.Y < 0 || neighbour.X >= PatchCoordinates.PatchWidth || neighbour.Y >= PatchCoordinates.PatchHeight)
                            {
                                continue;
                            }

                            this.patchOperationCache[neighbour.X, neighbour.Y] = TileType.Water;
                        }
                    }
                }
            }

            for (int i = 0; i < PatchCoordinates.PatchWidth; i++)
            {
                for (int j = 0; j < PatchCoordinates.PatchHeight; j++)
                {
                    if (this.patchOperationCache[i, j] == TileType.Water)
                    {
                        this.patchCache[i, j] = TileType.Water;
                    }
                }
            }

            return this.patchCache;
        }
    }
}

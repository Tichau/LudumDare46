using System;

namespace World
{
    public class Generator
    {
        private const int NoiseOctaveCount = 5;

        private Settings settings;
        private Tile[,] patchCache;
        private float[][,] noiseByOctave;

        public struct Settings
        {
            public int Seed;
            public float SeaLevel;
            public float ChaosLevel;
        }

        public Generator(Settings settings)
        {
            this.settings = settings;

            // Instantiate data structures.
            Random random = new Random(this.settings.Seed);

            this.patchCache = new Tile[PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight];
            float[,] whiteNoise = Noise.GenerateWhiteNoise(PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight, random);

            this.noiseByOctave = new float[NoiseOctaveCount][,];
        
            // Generate smooth noise
            for (int i = 0; i < NoiseOctaveCount; i++)
            {
                this.noiseByOctave[i] = Noise.GenerateSmoothNoise(whiteNoise, i, Noise.LinearInterpolation);
            }
        }

        public Tile[,] GenerateWorldPatch(PatchCoordinates patchCoordinates)
        {
            Debug.Assert(this.patchCache != null && this.patchCache.GetLength(0) == PatchCoordinates.PatchWidth && this.patchCache.GetLength(0) == PatchCoordinates.PatchHeight, "Invalid world patch");

            Random random = new Random(this.settings.Seed.GetHashCode() + patchCoordinates.GetHashCode());

            // Generate terrain type.
            float[,] perlinNoise = Noise.GeneratePerlinNoise(this.noiseByOctave, this.settings.ChaosLevel);

            for (int i = 0; i < PatchCoordinates.PatchWidth; i++)
            {
                for (int j = 0; j < PatchCoordinates.PatchHeight; j++)
                {
                    if (perlinNoise[i, j] > this.settings.SeaLevel)
                    {
                        this.patchCache[i, j] = Tile.Grass;
                    }
                    else
                    {
                        this.patchCache[i, j] = Tile.Water;
                    }
                }
            }

            return this.patchCache;
        }
    }
}

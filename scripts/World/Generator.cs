using System;

namespace World
{
    public class Generator
    {
        private const int NoiseOctaveCount = 5;

        private Settings settings;
        private TileType[,] patchCache;
        private float[][,] noiseByOctave;
        private float[,] floatCache;

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

            this.patchCache = new TileType[PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight];
            this.floatCache = new float[PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight];
            this.noiseByOctave = new float[NoiseOctaveCount][,];

            this.noiseByOctave[0] = Noise.GenerateWhiteNoise(PatchCoordinates.PatchWidth, PatchCoordinates.PatchHeight, random);
        }

        public TileType[,] GenerateWorldPatch(PatchCoordinates patchCoordinates)
        {
            Debug.Assert(this.patchCache != null && this.patchCache.GetLength(0) == PatchCoordinates.PatchWidth && this.patchCache.GetLength(1) == PatchCoordinates.PatchHeight, "Invalid world patch");

            Random random = new Random(this.settings.Seed.GetHashCode() + patchCoordinates.GetHashCode());

            // Generate terrain type.
            for (int octave = 1; octave < NoiseOctaveCount; octave++)
            {
                Noise.GenerateStretchNoise(this.noiseByOctave[0], patchCoordinates.X, patchCoordinates.Y, octave, Noise.LinearInterpolation, ref this.noiseByOctave[octave]);
            }

            Noise.GeneratePerlinNoise(this.noiseByOctave, this.settings.ChaosLevel, ref this.floatCache);

            for (int i = 0; i < PatchCoordinates.PatchWidth; i++)
            {
                for (int j = 0; j < PatchCoordinates.PatchHeight; j++)
                {
                    if (this.floatCache[i, j] > this.settings.SeaLevel)
                    {
                        this.patchCache[i, j] = TileType.Grass;
                    }
                    else
                    {
                        this.patchCache[i, j] = TileType.Water;
                    }
                }
            }

            return this.patchCache;
        }
    }
}

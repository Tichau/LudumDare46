using System;

public static class Noise
{
    public delegate float InterpolationFunction(float x0, float x1, float ratio);

    public static float[,] GenerateWhiteNoise(int width, int height, Random random)
    {
        float[,] noise = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noise[i, j] = (float)random.NextDouble();
            }
        }

        return noise;
    }

    public static float[,] GenerateSmoothNoise(float[,] baseNoise, int octave, InterpolationFunction interpolate)
    {
        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);
        
        float[,] smoothNoise = new float[width, height];
        
        int samplePeriod = 1 << octave; // calculates 2 ^ k
        float sampleFrequency = 1.0f / samplePeriod;
        
        for (int i = 0; i < width; i++)
        {
            // Calculate the horizontal sampling indices
            int sample_i0 = (i / samplePeriod) * samplePeriod;
            int sample_i1 = (sample_i0 + samplePeriod) % width; // Wrap around
            float horizontal_blend = (i - sample_i0) * sampleFrequency;
        
            for (int j = 0; j < height; j++)
            {
                // Calculate the vertical sampling indices
                int sample_j0 = (j / samplePeriod) * samplePeriod;
                int sample_j1 = (sample_j0 + samplePeriod) % height; //wrap around
                float vertical_blend = (j - sample_j0) * sampleFrequency;
        
                // Blend the top two corners
                float top = interpolate.Invoke(baseNoise[sample_i0, sample_j0], baseNoise[sample_i1, sample_j0], horizontal_blend);
        
                // Blend the bottom two corners
                float bottom = interpolate.Invoke(baseNoise[sample_i0, sample_j1], baseNoise[sample_i1, sample_j1], horizontal_blend);
        
                // Final blend
                smoothNoise[i, j] = interpolate.Invoke(top, bottom, vertical_blend);
            }
        }
        
        return smoothNoise;
    }

    public static void GenerateStretchNoise(float[,] baseNoise, int offsetX, int offsetY, int octave, InterpolationFunction interpolate, ref float[,] result)
    {
        int width = baseNoise.GetLength(0);
        int height = baseNoise.GetLength(1);
        
        if (result == null)
        {
            result = new float[width, height];
        }
        
        Debug.Assert(result.GetLength(0) == width && result.GetLength(1) == height, "Invalid array size");
        
        int samplePeriod = 1 << octave; // calculates 2 ^ k
        float sampleFrequency = 1.0f / samplePeriod;

        // Wrap the offset around the noise map size (handle negative offset as well).
        offsetX = (((offsetX % width) + width) % width) * (width / samplePeriod);
        offsetY = (((offsetY % height) + height) % height) * (height / samplePeriod);

        for (int i = 0; i < width; i++)
        {
            // Calculate the horizontal sampling indices
            int sample_i0 = (offsetX + i / samplePeriod) % width;
            int sample_i1 = (sample_i0 + 1) % width;
            float horizontal_blend = (i % samplePeriod) * sampleFrequency;
        
            for (int j = 0; j < height; j++)
            {
                // Calculate the vertical sampling indices
                int sample_j0 = (offsetY + j / samplePeriod) % height;
                int sample_j1 = (sample_j0 + 1) % height;
                float vertical_blend = (j % samplePeriod) * sampleFrequency;
        
                // Blend the top two corners
                float top = interpolate.Invoke(baseNoise[sample_i0, sample_j0], baseNoise[sample_i1, sample_j0], horizontal_blend);
        
                // Blend the bottom two corners
                float bottom = interpolate.Invoke(baseNoise[sample_i0, sample_j1], baseNoise[sample_i1, sample_j1], horizontal_blend);
        
                // Final blend
                result[i, j] = interpolate.Invoke(top, bottom, vertical_blend);
            }
        }
    }

    public static void GeneratePerlinNoise(float[][,] noiseByOctave, float persistance, ref float[,] result)
    {
        int octaveCount = noiseByOctave.Length;
        if (octaveCount == 0)
        {
            throw new System.ArgumentOutOfRangeException("noiseByOctave");
        }

        int width = noiseByOctave[0].GetLength(0);
        int height = noiseByOctave[0].GetLength(1);

        if (result == null)
        {
            result = new float[width, height];
        }
        
        Debug.Assert(result.GetLength(0) == width && result.GetLength(1) == height, "Invalid array size");
    
        // Reset data
        System.Array.Clear(result, 0, width * height);

        // Blend noise together
        float amplitude = 1.0f;
        float totalAmplitude = 0.0f;
        for (int octave = octaveCount - 1; octave >= 0; octave--)
        {
            amplitude *= persistance;
            totalAmplitude += amplitude;
        
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result[i, j] += noiseByOctave[octave][i,j] * amplitude;
                }
            }
        }
        
        // Normalisation
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                result[i, j] /= totalAmplitude;
            }
        }
    }

    public static float LinearInterpolation(float x0, float x1, float alpha)
    {
        return x0 * (1 - alpha) + alpha * x1;
    }
}

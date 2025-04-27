using System;
using UnityEngine;
using UnityEngine.Android;

public class Terrain_generator : MonoBehaviour
{
    [Header("Terrain Size")]
    [SerializeField] private int width = 256;
    [SerializeField] private int depth = 256;
    [SerializeField] private int height = 50;

    [Header("Noise Settings")]
    [SerializeField] private float scale = 20f;
    [SerializeField] private int octaves = 4;
    [SerializeField] private float persistence = 0.5f;
    [SerializeField] private float lacunarity = 2f;
    [SerializeField] private Vector2 offset;

    [Header("Height Adjustment")]
    [SerializeField] private AnimationCurve heightCurve;
    [SerializeField] private float heightMultiplier = 1f;

    // Private vars
    private Terrain terrain;
    private int lastDepth, lastWidth, lastHeight, lastOctaves;
    private float lastScale, lastPersistence, lastLacunarity, lastheightMultiplier;


    void Start()
    {
        terrain = GetComponent<Terrain>();
        if (terrain == null) {
            throw new NullReferenceException("Terrain not specified");
        }
        terrain.terrainData = GenerateTerrain();
        lastDepth = depth;
        lastWidth = width;
        lastHeight = height;
        lastScale = scale;
        lastOctaves = octaves;
        lastLacunarity = lacunarity;
        lastPersistence = persistence; 
        lastheightMultiplier = heightMultiplier;
    }

    TerrainData GenerateTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, height, depth);

        float[,] heights = GenerateHeights();

        terrainData.SetHeights(0, 0, heights);
        return terrainData;
    }

    void Update()
    {
                // Check if size parameters changed
        if (depth != lastDepth || width != lastWidth || height != lastHeight || scale != lastScale || persistence != lastPersistence || octaves != lastOctaves || lacunarity != lastLacunarity || heightMultiplier != lastheightMultiplier)
        {
            Debug.Log("Terrain size changed! Regenerating terrain...");

            GenerateTerrain();

            // Update last known values
            lastDepth = depth;
            lastWidth = width;
            lastHeight = height;
            lastScale = scale;
            lastPersistence = persistence;
            lastOctaves = octaves;
            lastLacunarity = lacunarity;
            lastheightMultiplier = heightMultiplier;
        }
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                heights[x, z] = CalculateHeight(x, z);
                // CalculateHeight(x, z);
            }
        }
        return heights;
    }

    // private float CalculateTiledHeight(float x, float y)
    // {
    //     float amplitude = 1;
    //     float frequency = 1;
    //     float noiseHeight = 0;
    //     for (int i = 0; i < octaves; i++)
    //     {
    //         // Wrap x and z into angles
    //         float xAngle = ((x + offset.x) / (width - 1)) * 2f * Mathf.PI * frequency;
    //         float zAngle = ((z + offset.y) / (depth - 1)) * 2f * Mathf.PI * frequency;

    //         // Circle mapping for seamless tiling
    //         float sampleX = Mathf.Cos(xAngle) * scale;
    //         float sampleY = Mathf.Sin(xAngle) * scale;
    //         float sampleZ = Mathf.Cos(zAngle) * scale;
    //         float sampleW = Mathf.Sin(zAngle) * scale;

    //         // 4D noise lookup
    //         float perlinValue = NotSoSimplePerlin.Noise4D(sampleX, sampleY, sampleZ, sampleW) * 2 - 1;

    //         noiseHeight += perlinValue * amplitude;

    //         amplitude *= persistence;
    //         frequency *= lacunarity;
    //     }

    //     noiseHeight = heightCurve.Evaluate((noiseHeight + 1) / 2f) * heightMultiplier;
    //     return noiseHeight;




    //     // float sampleX, sampleZ, perlinValue;

    //     // for (int i = 0; i < octaves; i++)
    //     // {
    //     //     sampleX = (x + offset.x) / scale * frequency;
    //     //     sampleZ = (z + offset.y) / scale * frequency;

    //     //     perlinValue = NotSoSimplePerlin.Noise(sampleX, sampleZ) * 2 - 1;
    //     //     noiseHeight += perlinValue * amplitude;

    //     //     amplitude *= persistence;
    //     //     frequency *= lacunarity;
    //     // }

    //     // noiseHeight = heightCurve.Evaluate((noiseHeight + 1) / 2f) * heightMultiplier;
    //     // return noiseHeight;
    //     // // Angle goes from 0 to 2*PI across the terrain width/height
    //     // float xAngle = (x / (width - 1)) * 2f * Mathf.PI;
    //     // float yAngle = (y / (height - 1)) * 2f * Mathf.PI;

    //     // // Radius controls how big the loops are
    //     // float radius = scale;

    //     // // Map angles into a seamless circle
    //     // float sampleX = Mathf.Cos(xAngle) * radius;
    //     // float sampleY = Mathf.Sin(xAngle) * radius;
    //     // float sampleZ = Mathf.Cos(yAngle) * radius;
    //     // float sampleW = Mathf.Sin(yAngle) * radius;

    //     // // You need a noise function that can take 4D input here
    //     // return NotSoSimplePerlin.Noise4D(sampleX, sampleY, sampleZ, sampleW);
    // }

    private float CalculateHeight(int x, int z)
    {
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        float sampleX, sampleZ, perlinValue;

        for (int i = 0; i < octaves; i++)
        {
            sampleX = (x + offset.x) / scale * frequency;
            sampleZ = (z + offset.y) / scale * frequency;

            perlinValue = NotSoSimplePerlin.Noise(sampleX, sampleZ) * 2 - 1;
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        noiseHeight = heightCurve.Evaluate((noiseHeight + 1) / 2f) * heightMultiplier;
        return noiseHeight;
    }

    // private float CalculateHeightAdvanced(int x, int z)
    // {
    //     float amplitude = 1;
    //     float frequency = 1;
    //     float noiseHeight = 0;

    //     for (int i = 0; i < octaves; i++)
    //     {
    //         float xAngle = ((x + offset.x) / (width - 1)) * 2f * Mathf.PI * frequency;
    //         float zAngle = ((z + offset.y) / (depth - 1)) * 2f * Mathf.PI * frequency;

    //         float sampleX = Mathf.Cos(xAngle) * scale;
    //         float sampleY = Mathf.Sin(xAngle) * scale;
    //         float sampleZ = Mathf.Cos(zAngle) * scale;
    //         float sampleW = Mathf.Sin(zAngle) * scale;

    //         // Combine samples into 2D for NotSoSimplePerlin
    //         float perlinX = sampleX + sampleZ;
    //         float perlinY = sampleY + sampleW;

    //         float perlinValue = NotSoSimplePerlin.Noise(perlinX, perlinY) * 2f - 1f;
    //         // ADD THIS: Make it more mountain-like
    //         perlinValue = 1f - Mathf.Abs(perlinValue); // Turn into ridges
    //         perlinValue = Mathf.Pow(perlinValue, 0.6f); // Sharpen mountains

    //         noiseHeight += perlinValue * amplitude;

    //         amplitude *= persistence;
    //         frequency *= lacunarity;
    //     }

    //     noiseHeight = heightCurve.Evaluate((noiseHeight + 1f) / 2f) * heightMultiplier;
    //     return noiseHeight;
    // }

// private float CalculateHeightAdvanced(int x, int z)
// {
//     float amplitude = 1;
//     float frequency = 1;
//     float noiseHeight = 0;

//     float smoothNoise = 0f;
//     float ridgeNoise = 0f;

//     float sampleX, sampleZ, sampleW;
//     float perlinValue;

//     float timeW = Time.time * 0.1f;

//     for (int i = 0; i < octaves; i++)
//     {
//         sampleX = (x + offset.x) / scale * frequency;
//         sampleZ = (z + offset.y) / scale * frequency;
//         sampleW = timeW * frequency;

//         perlinValue = NotSoSimplePerlin.Noise(sampleX, sampleZ, sampleW, 0f) * 2 - 1;

//         smoothNoise += perlinValue * amplitude;

//         // Ridge noise
//         float ridge = 1f - Mathf.Abs(perlinValue);
//         ridge = ridge * ridge; // square to make peaks sharper
//         ridgeNoise += ridge * amplitude;

//         amplitude *= persistence;
//         frequency *= lacunarity;
//     }

//     // Blend smooth hills + mountains
//     float blendFactor = 0.5f; // 0 = only smooth, 1 = only mountains
//     noiseHeight = Mathf.Lerp(smoothNoise, ridgeNoise, blendFactor);

//     noiseHeight = heightCurve.Evaluate((noiseHeight + 1) / 2f) * heightMultiplier;
//     return noiseHeight;
// }


}

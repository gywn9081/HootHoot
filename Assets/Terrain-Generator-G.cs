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
    private int lastDepth, lastWidth, lastHeight;
    private float lastScale;

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
        if (depth != lastDepth || width != lastWidth || height != lastHeight || scale != lastScale)
        {
            Debug.Log("Terrain size changed! Regenerating terrain...");

            GenerateTerrain();

            // Update last known values
            lastDepth = depth;
            lastWidth = width;
            lastHeight = height;
            lastScale = scale;
        }
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                heights[x, z] = CalculateHeight(x, z);
            }
        }
        return heights;
    }

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
}

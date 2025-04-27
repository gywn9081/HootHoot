using System;
using UnityEngine;
using UnityEngine.Android;

[RequireComponent(typeof(Terrain))]
public class TerrainGenerator : MonoBehaviour
{

    [SerializeField] public int depth;
    [SerializeField] public int width;
    [SerializeField] public int height;
    public float scale = 2f;
    private Terrain terrain;
    private int lastDepth, lastWidth, lastHeight;
    private float lastScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        // terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        float[,] heights = GenerateHeights();

        terrainData.SetHeights(0, 0, heights);
        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                heights[i, j] = CalculateHeights(i, j);
            }
        }
        return heights;
    }

    private float CalculateHeights(float x, float y)
    {
        float xcoord = (float)x / width * scale;
        float ycoord = (float)y / height * scale;

        return NotSoSimplePerlin.Noise(xcoord, ycoord);
    }


    // // Update is called once per frame
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
}


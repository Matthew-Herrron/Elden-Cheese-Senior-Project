using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PGCTerrain : MonoBehaviour
{
    public Terrain terrain;
    public TerrainData myTerrainData;
    public float randomHeightRange;
    public Vector3 heightMapScale;
    public Texture2D heightMapImage;
    float[,] heightMap;

   

    public int width = 1000;
    public int height = 1000;
    public int depth = 1000;

    //Sine Variables
    public float period;
    public float amplitude;
    public float frequency = 1f;

    //perlin variables
    public float perlinHeight = 1f;
    public float perlinX = 1f;
    public float perlinY = 1f;

    void Start()
    {
        randomHeightRange = 0.0f;

    }

    void OnEnable()
    {
        terrain = this.GetComponent<Terrain>();
        myTerrainData = Terrain.activeTerrain.terrainData;
        myTerrainData.size = new Vector3(width, depth, height);
        myTerrainData.baseMapResolution = depth;
        heightMap = myTerrainData.GetHeights(0, 0, myTerrainData.heightmapResolution, myTerrainData.heightmapResolution);
    }
    
    //implement random terrain 
    public void RandomTerrain()
    {
        //init height map
        heightMap = myTerrainData.GetHeights(0, 0, myTerrainData.heightmapResolution, myTerrainData.heightmapResolution);

        for (int x = 0; x < myTerrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < myTerrainData.heightmapResolution; z++)
            {
                heightMap[x, z] += Random.Range(-randomHeightRange, randomHeightRange);
            }
        }
        myTerrainData.SetHeights(0, 0, heightMap);
    }
    //implementation for sin curve
    public void CosTerrain()
    {
        //init height map
        heightMap = myTerrainData.GetHeights(0, 0, myTerrainData.heightmapResolution, myTerrainData.heightmapResolution);

        for (int x = 0; x < myTerrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < myTerrainData.heightmapResolution; z++)
            {
                heightMap[x, z] += Mathf.Cos(((x * frequency) + z) * (period / 10f)) * amplitude;
            }
        }
        myTerrainData.SetHeights(0, 0, heightMap);
    }

    //implement perlin noise
    
     public void PerlinTerrain()
    {
        //init height map
        heightMap = myTerrainData.GetHeights(0, 0, myTerrainData.heightmapResolution, myTerrainData.heightmapResolution);

        for (int x = 0; x < myTerrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < myTerrainData.heightmapResolution; y++)
            {
                heightMap[x, y] += (perlinHelper(x, y) * (perlinHeight));
            }

        }
        myTerrainData.SetHeights(0, 0, heightMap);
    }

    
    public void ResetTerrain()
    {
        //init height map
        heightMap = myTerrainData.GetHeights(0, 0, myTerrainData.heightmapResolution, myTerrainData.heightmapResolution);
        for (int x = 0; x < myTerrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < myTerrainData.heightmapResolution; z++)
            {
                //set all values to 0
                heightMap[x, z] = 0f;
            }
        }
        myTerrainData.SetHeights(0, 0, heightMap);
    }

    float perlinHelper(float x, float y)
    {
        float xCord = (float)x / width * perlinX;
        float yCord = (float)y / height * perlinY;

        return Mathf.PerlinNoise(xCord, yCord);
    }
     
}

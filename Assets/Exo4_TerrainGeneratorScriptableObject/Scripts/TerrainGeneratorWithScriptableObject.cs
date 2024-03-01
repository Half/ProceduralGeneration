using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGeneratorWithScriptableObject : MonoBehaviour {

    [SerializeField]
    private int seed;

    [SerializeField]
    private int terrainSizeX = 50;

    [SerializeField]
    private int terrainSizeZ = 50;

    [SerializeField]
    private List<TerrainBiome> biomes = new List<TerrainBiome>();

    private TerrainBiome[,] terrain;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        GenerateTerrain();
    }

    public void BtnGenerateTerrainWithRandomSeed() {
        seed = Random.Range(int.MinValue, int.MaxValue);
        GenerateTerrain();
    }

    public void BtnGenerateTerrain() {
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {

        terrain = new TerrainBiome[terrainSizeX, terrainSizeZ];

        // Init seed
        Random.InitState(seed);

        foreach (TerrainBiome biome in biomes) {
            


        }

        // Mountain
        Generate(TILE_TYPE.MOUNTAIN, new float2(20f, 40f), NOISE_TYPE.PERLIN, 0.8f);

        // Forest
        Generate(TILE_TYPE.FOREST, new float2(20f, 40f), NOISE_TYPE.SIMPLEX, 0.5f);

        // Water
        Generate(TILE_TYPE.WATER, new float2(50f, 70f), NOISE_TYPE.CELLULAR, 0.8f);

        Spawn();

    }

    private void Generate(TILE_TYPE type, float2 scaleMinMax, NOISE_TYPE noiseType, float greaterThanValue) {

        // Forest
        float2 noiseOffset = new float2(Random.Range(0f, 100000f), Random.Range(0f, 100000f));
        float noiseScale = Random.Range(scaleMinMax.x, scaleMinMax.y);

        for (int x = 0; x < terrainSizeX; x++)
        {
            for (int y = 0; y < terrainSizeZ; y++)
            {

                float2 coordsOffseted = new float2(x + noiseOffset.x, y + noiseOffset.y);
                float2 coords = new float2(coordsOffseted.x / noiseScale, coordsOffseted.y / noiseScale);

                float noiseValue = GetNoiseValue(noiseType, coords);

                if (noiseValue > greaterThanValue)
                {
                    terrain[x, y] = type;
                }

            }
        }

    }

    private void Spawn() {

        foreach (GameObject spawnedObject in spawnedObjects) {
            Destroy(spawnedObject);
        }

        for (int x = 0; x < terrainSizeX; x++)
        {
            for (int y = 0; y < terrainSizeZ; y++) {

                GameObject prefab = GetPrefab(terrain[x, y]);
                GameObject go = Instantiate(prefab, new Vector3(x, 0f, y), Quaternion.identity);
                spawnedObjects.Add(go);

            }
        }

    }

    private GameObject GetPrefab(TILE_TYPE type) {

        switch (type) {
            case TILE_TYPE.GRASS:
                return prGrass;
            case TILE_TYPE.MOUNTAIN:
                return prMountain;
            case TILE_TYPE.FOREST:
                return prForest;
            case TILE_TYPE.WATER:
                return prWater;
        }

        return null;

    }

    private float GetNoiseValue(NOISE_TYPE noiseType, float2 coords)
    {

        float value = 0.5f;

        switch (noiseType)
        {
            case NOISE_TYPE.CELLULAR:
                float2 res = noise.cellular(coords);
                value = res.x;
                break;

            case NOISE_TYPE.PERLIN:
                value = noise.cnoise(coords);
                break;

            case NOISE_TYPE.SIMPLEX:
                value = noise.snoise(coords);
                break;
        }

        value = math.remap(-1f, 1f, 0f, 1f, value);

        return value;

    }

}

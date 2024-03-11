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
    private List<TerrainBiomeData> biomes = new List<TerrainBiomeData>();

    private TerrainBiomeData[,] terrain;

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

        terrain = new TerrainBiomeData[terrainSizeX, terrainSizeZ];

        // Init seed
        Random.InitState(seed);

        // Generate terrain
        foreach (TerrainBiomeData biome in biomes) {
            biome.Generate(ref terrain);
        }

        // Spawn
        Spawn();

    }

    private void Spawn() {

        foreach (GameObject spawnedObject in spawnedObjects) {
            Destroy(spawnedObject);
        }

        for (int x = 0; x < terrainSizeX; x++) {
            for (int y = 0; y < terrainSizeZ; y++) {

                GameObject prefab = terrain[x, y].GetPrefabToSpawn();
                GameObject go = Instantiate(prefab, new Vector3(x, 0f, y), Quaternion.identity);
                go.transform.SetParent(transform);
                spawnedObjects.Add(go);

            }
        }

    }

}

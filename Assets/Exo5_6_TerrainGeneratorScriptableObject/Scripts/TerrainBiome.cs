using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Terrain Biome", fileName = "Terrain Biome")]
public class TerrainBiome : ScriptableObject {

    
    [SerializeField]
    private List<GameObject> spawnablePrefabs = new List<GameObject>();

    [SerializeField]
    private List<TerrainBiomeNoise> noises = new List<TerrainBiomeNoise>();

    [SerializeField, Range(0f, 1f)]
    private float validIfGreaterThan;

    public void Generate(ref TerrainBiome[,] terrain) {

        // Setup temp noise array
        float[,] noiseValues = new float[terrain.GetLength(0), terrain.GetLength(1)];

        // Get all noise values
        foreach (TerrainBiomeNoise noise in noises) {

            float2 noiseOffset = new float2(Random.Range(noise.offsetMinMax.x, noise.offsetMinMax.y), Random.Range(noise.offsetMinMax.x, noise.offsetMinMax.y));
            float noiseScale = Random.Range(noise.scaleMinMax.x, noise.scaleMinMax.y);

            for (int x = 0; x < noiseValues.GetLength(0); x++) {
                for (int y = 0; y < noiseValues.GetLength(1); y++) {

                    float2 coordsOffseted = new float2(x + noiseOffset.x, y + noiseOffset.y);
                    float2 coords = new float2(coordsOffseted.x / noiseScale, coordsOffseted.y / noiseScale);

                    noiseValues[x, y] += noise.GetNoiseValue(coords) / noises.Count;

                }
            }

        }

        // Apply noise on terrain
        for (int x = 0; x < terrain.GetLength(0); x++) {
            for (int y = 0; y < terrain.GetLength(1); y++) {

                if (noiseValues[x, y] >= validIfGreaterThan) {
                    terrain[x, y] = this;           
                }

            }
        }

    }

    public GameObject GetPrefabToSpawn() {

        return spawnablePrefabs[Random.Range(0, spawnablePrefabs.Count)];

    }

}

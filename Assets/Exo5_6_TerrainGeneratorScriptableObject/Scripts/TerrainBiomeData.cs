using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Terrain Biome", fileName = "Terrain Biome")]
public class TerrainBiomeData : ScriptableObject {

    [SerializeField]
    private List<GameObject> spawnablePrefabs = new List<GameObject>();

    [SerializeField]
    private List<TerrainBiomeNoise> noises = new List<TerrainBiomeNoise>();

    [SerializeField, Range(0f, 1f)]
    private float validIfGreaterThan;

    [SerializeField]
    private AnimationCurve curveX;

    [SerializeField]
    private AnimationCurve curveZ;

    public void Generate(ref TerrainBiomeData[,] terrain) {

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

                    float value = noise.GetNoiseValue(coords) / noises.Count;
                    value *= curveX.Evaluate(x / (float)noiseValues.GetLength(0));
                    value *= curveZ.Evaluate(y / (float) noiseValues.GetLength(1));

                    noiseValues[x, y] += value;

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

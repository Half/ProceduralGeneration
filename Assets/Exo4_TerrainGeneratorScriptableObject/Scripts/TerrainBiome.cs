using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TerrainBiome : ScriptableObject {

    
    [SerializeField]
    private List<GameObject> spawnablePrefabs = new List<GameObject>();

    [SerializeField]
    private List<TerrainBiomeNoise> noises = new List<TerrainBiomeNoise>();

    [SerializeField, Range(0f, 1f)]
    private float validIfGreaterThan;

    public float2 GetNoiseOffset() {
        


    }

    public bool IsValid(int x, int y) {

        float noiseValue = 1f;

        foreach (TerrainBiomeNoise noise in noises) {

            float2 noiseOffset = new float2(Random.Range(noise.offsetMinMax.x, noise.offsetMinMax.y), Random.Range(noise.offsetMinMax.x, noise.offsetMinMax.y));
            float noiseScale = Random.Range(noise.scaleMinMax.x, noise.scaleMinMax.y);

            float2 coordsOffseted = new float2(x + noiseOffset.x, y + noiseOffset.y);
            float2 coords = new float2(coordsOffseted.x / noiseScale, coordsOffseted.y / noiseScale);

            noiseValue *= noise.GetNoiseValue(coords);

        }

        if (noiseValue >= validIfGreaterThan) {
            return true;
        }

        return false;

    }

}

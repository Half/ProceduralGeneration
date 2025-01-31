using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct TerrainBiomeNoise {

    public enum TYPE {
        CELLULAR,
        PERLIN,
        SIMPLEX,
        NOISE_X_5
    }

    [SerializeField] public TYPE noiseType;

    [SerializeField] public float2 scaleMinMax;

    [SerializeField] private float2 offsetMinMax;

    [SerializeField] private bool invert;

    public float2 GetNoiseOffset() {

        return new float2(Random.Range(offsetMinMax.x, offsetMinMax.y), Random.Range(offsetMinMax.x, offsetMinMax.y));

    }


    public float GetNoiseValue(float2 coords) {

        float value = 0f;

        switch (noiseType) {
            case TYPE.CELLULAR:
                float2 res = noise.cellular(coords);
                value = res.x;
                break;

            case TYPE.PERLIN:
                value = noise.cnoise(coords);
                value = math.remap(-1f, 1f, 0f, 1f, value);
                break;

            case TYPE.SIMPLEX:
                value = noise.snoise(coords);
                value = math.remap(-1f, 1f, 0f, 1f, value);
                break;

            case TYPE.NOISE_X_5:

                if (Mathf.Approximately(coords.x, 5f)) {
                    value = 1f;
                }

                else {
                    value = 0f;
                }

                break;
        }

        if (invert) {
            value = 1f - value;
        }

        return value;

    }

}

using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct TerrainBiomeNoise {

    public enum TYPE {
        CELLULAR,
        PERLIN,
        SIMPLEX,
    }

    [SerializeField] public TYPE noiseType;

    [SerializeField] public float2 scaleMinMax;

    [SerializeField] public float2 offsetMinMax;

    [SerializeField] public bool invert;

    public float GetNoiseValue(float2 coords) {

        float value = 0f;

        switch (noiseType) {
            case TYPE.CELLULAR:
                float2 res = noise.cellular(coords);
                value = res.x;
                break;

            case TYPE.PERLIN:
                value = noise.cnoise(coords);
                break;

            case TYPE.SIMPLEX:
                value = noise.snoise(coords);
                break;
        }

        value = math.remap(-1f, 1f, 0f, 1f, value);

        if (invert) {
            value = 1f - value;
        }

        return value;

    }

}

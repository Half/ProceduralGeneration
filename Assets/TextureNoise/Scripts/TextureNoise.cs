using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TextureNoise : MonoBehaviour {

    [SerializeField]
    private Renderer targetRenderer;

    [SerializeField]
    private int textureSizeX = 1024;

    [SerializeField]
    private int textureSizeY = 1024;

    public enum NOISE_TYPE {
        CELLULAR,
        PERLIN,
        SIMPLEX,
    }

    [SerializeField]
    private NOISE_TYPE noiseType;

    [SerializeField]
    private float2 noiseOffset;

    [SerializeField]
    private float noiseScale = 100f;

    [SerializeField]
    private float resultPow = 1f;

    private Texture2D tex;

    // Start is called before the first frame update
    IEnumerator Start() {

        yield return CrtGenerate();
    
    }

    public void Generate() {
        StopAllCoroutines();
        StartCoroutine(CrtGenerate());
    }

    private IEnumerator CrtGenerate() {

        tex = new Texture2D(textureSizeX, textureSizeY);
        targetRenderer.material.mainTexture = tex;

        for (int x = 0; x < textureSizeX; x++) {
            for (int y = 0; y < textureSizeY; y++) {

                float2 coordsOffseted = new float2(x + noiseOffset.x, y + noiseOffset.y);
                float2 coords = new float2(coordsOffseted.x / noiseScale, coordsOffseted.y / noiseScale);

                float color = GetColor(coords);
                color = ApplyAlgo(color);

                tex.SetPixel(x, y, Color.white * color);

            }
        }

        tex.Apply();

        yield break;

    }

    private float GetColor(float2 coords) {

        float color = 0.5f;

        switch (noiseType) {

            case NOISE_TYPE.CELLULAR:
                float2 res = noise.cellular(coords);
                color = res.x;
                break;

            case NOISE_TYPE.PERLIN:
                color = noise.cnoise(coords);
                break;

            case NOISE_TYPE.SIMPLEX:
                color = noise.snoise(coords);
                break;
        }

        //color += 1f;
        //color /= 2f;

        color = math.remap(-1f, 1f, 0f, 1f, color);

        return color;

    }

    private float ApplyAlgo(float color) {

        float res = math.pow(color, resultPow);

        return res;

    }

}

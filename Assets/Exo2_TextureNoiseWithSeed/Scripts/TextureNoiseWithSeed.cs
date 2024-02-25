using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class TextureNoiseWithSeed : MonoBehaviour {

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
    private int seed;

    [SerializeField]
    private float resultPow = 1f;

    private Texture2D tex;

    // Start is called before the first frame update
    IEnumerator Start() {
        
        BtnGenerate();
        yield break;

        //yield return StartCoroutine(TestLog());

        //yield return StartCoroutine(TestLog2());

        //StopAllCoroutines();

    }

    private IEnumerator TestLog() {
        
        Debug.Log("Start");
        yield return new WaitForSeconds(5f);
        Debug.Log("Start 0.5");

        for (int i = 0; i < 5; i++) {
            yield return 0f;
            //if (1 == 1) {
            //    yield break;
            //}
        }
        StartCoroutine(TestLog2());
        yield return 0f;
        yield return StartCoroutine(TestLog2());
        Debug.Log("Start next frame");

    }

    private IEnumerator TestLog2() {

        Debug.Log("Start");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Start 0.5");

        for (int i = 0; i < 5; i++) {
            yield return 0f;
            //if (1 == 1) {
            //    yield break;
            //}
        }

        yield return 0f;
        Debug.Log("Start next frame");

    }

    public void BtnGenerate() {
        StopAllCoroutines();
        StartCoroutine(CrtGenerate(seed));
    }

    public void BtnGenerateSeed() {
        StopAllCoroutines();
        seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        StartCoroutine(CrtGenerate(seed));
    }

    private IEnumerator CrtGenerate(int seed) {

        // Init seed
        UnityEngine.Random.InitState(seed);

        // Random
        float2 noiseOffset = new float2(
            UnityEngine.Random.Range(-100000f, 100000f),
            UnityEngine.Random.Range(-100000f, 100000f)
        );

        float noiseScale = UnityEngine.Random.Range(80f, 300f);

        tex = new Texture2D(textureSizeX, textureSizeY);
        targetRenderer.material.mainTexture = tex;

        int iteration = 0;
        int interationSteps = Mathf.RoundToInt((textureSizeX * textureSizeY) / 100f);

        for (int x = 0; x < textureSizeX; x++) {
            for (int y = 0; y < textureSizeY; y++) {

                float2 coordsOffseted = new float2(x + noiseOffset.x, y + noiseOffset.y);
                float2 coords = new float2(coordsOffseted.x / noiseScale, coordsOffseted.y / noiseScale);

                float color = GetColor(coords);
                color = ApplyAlgo(color);

                tex.SetPixel(x, y, Color.white * color);

                iteration++;            
                if (iteration % interationSteps == 0) {
                    tex.Apply();
                    yield return new WaitForSeconds(0.05f);
                }

            }
        }

        tex.Apply();

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

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PifShuffleBagGeneric : MonoBehaviour {

    [SerializeField]
    private GameObject prefabGraphItem;

    [SerializeField]
    private int nbRandomToGenerate;

    [SerializeField]
    private int randomMax = 100;

    private int nbRandom;

    private GameObject[] graphItem;
    private int[] graphData;

    private ShuffleBagGeneric<int> shuffleBag;

    private int GetSize() {
        return randomMax;
    }

    private void Start() {

        graphItem = new GameObject[GetSize()];
        graphData = new int[GetSize()];
        shuffleBag = new ShuffleBagGeneric<int>();

        for (int i = 0; i < GetSize(); i++) {
            graphItem[i] = Instantiate(prefabGraphItem, transform);
            graphItem[i].transform.localPosition = new Vector3(i, 0f, 0f);
            graphData[i] = 0;

            shuffleBag.AddElement(i);
        }

        StartCoroutine(AddRandom(10));

    }

    private IEnumerator AddRandom(int nb) {

        nbRandom += nb;

        for (int i = 0; i < nb; i++) {

            int value = shuffleBag.Pick();
            graphData[value]++;

        }

        RefreshGraph();

        if (nbRandom >= nbRandomToGenerate) {
            yield break;
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(AddRandom(10));

    }

    private void RefreshGraph() {

        for (int i = 0; i < GetSize(); i++) {

            float scale = graphData[i];
            graphItem[i].transform.localScale = new Vector3(1f, scale, 1f);

        }

    }

}

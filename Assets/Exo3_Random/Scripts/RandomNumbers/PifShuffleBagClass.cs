using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PifShuffleBagClass : MonoBehaviour {

    [SerializeField]
    private GameObject prefabGraphItem;

    [SerializeField]
    private int nbRandomToGenerate;

    [SerializeField]
    private int randomMax = 100;

    private int nbRandom;

    private GameObject[] graphItem;
    private int[] graphData;

    private ShuffleBag shuffleBag;

    private int GetSize() {
        return randomMax;
    }

    private void Start() {

        // ----------------------

        ShuffleBag bag = new ShuffleBag();
        bag.AddElement(0);
        bag.AddElement(1);
        bag.AddElement(2);

        int value1 = bag.Pick();
        int value2 = bag.Pick();
        int value3 = bag.Pick();

        // ----------------------


        graphItem = new GameObject[GetSize()];
        graphData = new int[GetSize()];
        shuffleBag = new ShuffleBag();

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

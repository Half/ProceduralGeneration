using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PifShuffleBag : MonoBehaviour {

    [SerializeField]
    private GameObject prefabGraphItem;

    [SerializeField]
    private int nbRandomToGenerate;

    [SerializeField]
    private int randomMax = 100;

    private List<int> shuffleBag = new List<int>();

    private int[] graphData;

    private int nbRandom;

    private GameObject[] graphItem;

    private int GetSize() {
        return randomMax + 1;
    }

    private void Start() {

        graphData = new int[GetSize()];
        graphItem = new GameObject[GetSize()];

        for (int i = 0; i < GetSize(); i++) {

            graphItem[i] = Instantiate(prefabGraphItem, transform);
            graphItem[i].transform.localPosition = new Vector3(i, 0f, 0f);

        }

        StartCoroutine(AddRandom(10));

    }

    public static void Shuffle<T>(IList<T> list) {
        for (int i = 0; i < list.Count; i++) {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private int GetItemInShuffleBag() {

        if (shuffleBag.Count == 0) {

            for (int i = 0; i < GetSize(); i++) {
                shuffleBag.Add(i);
            }

            Shuffle(shuffleBag);

        }

        int random = shuffleBag[0];
        shuffleBag.RemoveAt(0);
        return random;

    }


    private IEnumerator AddRandom(int nb) {

        nbRandom += nb;

        for (int i = 0; i < nb; i++) {

            int random = GetItemInShuffleBag();
            graphData[random]++;

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

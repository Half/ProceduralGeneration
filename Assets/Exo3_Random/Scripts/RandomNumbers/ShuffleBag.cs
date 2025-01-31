using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleBag {

    private List<int> elements = new List<int>();

    private List<int> contents = new List<int>();

    public void AddElement(int value) {
        elements.Add(value);
        contents.Add(value);
    }

    public int Pick() {

        // Fill the bag if empty
        if (contents.Count == 0) {
            contents.AddRange(elements);
        }

        // Pick the value
        int index = Random.Range(0, contents.Count);
        int value = contents[index];

        // Remove the item
        contents.RemoveAt(index);

        return value;
    }

}

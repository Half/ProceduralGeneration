using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleBagGeneric<T> {

    private List<T> elements = new List<T>();

    private List<T> contents = new List<T>();

    public void AddElement(T value) {
        elements.Add(value);
        contents.Add(value);
    }

    public T Pick() {

        // Fill the bag if empty
        if (contents.Count == 0) {
            contents.AddRange(elements);
        }

        // Pick the value
        int index = Random.Range(0, contents.Count);
        T value = contents[index];

        // Remove the item
        contents.RemoveAt(index);

        return value;
    }

}

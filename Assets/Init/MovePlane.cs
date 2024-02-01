using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlane : MonoBehaviour {

    [SerializeField]
    private float speed = 0.1f;

    [SerializeField]
    private List<float> testList = new List<float>();

    public enum NOISE {
        PERLIN,
        SIMPLEX,
        CELLULAR
    }

    public NOISE testEnum;

    void Awake() {
        Debug.Log("AWake");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }

    void OnEnable() {
        Debug.Log("OnEnable");
    }

    void OnDisable() {
        Debug.Log("OnDisable");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update");

        float deltaTime = Time.deltaTime;
        float posX = transform.position.x + (speed * deltaTime);

        transform.position = new Vector3(posX, 0f, 0f);

    }

    void FixedUpdate() {
        
    }

    void OnDestroy() {
        
    }

}

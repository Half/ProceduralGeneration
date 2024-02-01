using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Manager : MonoBehaviour {

    [SerializeField]
    private int life;

    public int Life {
        get { return life; }
        private set {
            if (value < 0) {
                Debug.LogError("Je veux pas de nb négatif pls");
                value = 0;
            }

            OnLifeChanging?.Invoke(life, value);
            life = value;
            OnLifeChanged?.Invoke();

            //if (OnLifeChanged != null) {
            //    OnLifeChanged.Invoke();
            //}
        }
    }

    public event System.Action<int, int> OnLifeChanging;

    public event System.Action OnLifeChanged;

    [SerializeField]
    private List<Character> characters = new List<Character>();

    //[SerializeField]
    //private float speed = 0.1f;

    //public UnityEvent<int> testEvent;

    // Start is called before the first frame update
    void Start() {

        Life = 10;

        Debug.Log(Life);

        OnLifeChanging += LifeChanging;
        OnLifeChanged += LifeChanged;

    }

    public void TestEventUnity(int test) {
        
    }

    void LifeChanging(int oldLifeValue, int newLifeValue) {
        
    }

    void LifeChanged() {
        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Character character in characters) {

            float deltaTime = Time.deltaTime;
            float posX = character.transform.position.x + (character.Speed * deltaTime);

            character.transform.position = new Vector3(posX, character.transform.position.y, character.transform.position.z);

        }

        //for (int i = 0; i < moveableObjects.Count; i++) {

        //    Transform tr = moveableObjects[i];

        //}
    }

    void OnDestroy() {

        OnLifeChanging -= LifeChanging;
        OnLifeChanged -= LifeChanged;

    }
}

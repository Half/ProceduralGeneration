using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueScenario : MonoBehaviour {

    [SerializeField]
    private PlayableDirector dialogue;

    [SerializeField]
    private CinemachineVirtualCamera virtualCameraIso;

    private bool keySpacePressed;

    private IEnumerator Start() {

        virtualCameraIso.enabled = true;

        yield return new WaitUntil(() => keySpacePressed);

        virtualCameraIso.enabled = false;
        dialogue.Play();

        yield return new WaitWhile(() => {
            return dialogue.state == PlayState.Playing;
        });

        virtualCameraIso.enabled = true;

    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            keySpacePressed = true;
        }

    }

}

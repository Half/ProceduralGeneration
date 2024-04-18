using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CameraControlerIso : MonoBehaviour
{

    // Camera
    [Header("Camera")]
    [SerializeField]
    private Camera sourceCamera;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private Transform target;
    public Transform Target => target;

    private enum STATE { NONE, DRAGGING }

    // Current camera behaviour
    private STATE state;

    // Move
    private Vector3 lastCursorPos;

    // Zoom
    [Header("Zoom")]
    [SerializeField]
    private float zoomSensibility;

    [SerializeField]
    private float2 zoomRange = new float2(-10f, -20f);

    [SerializeField]
    private float zoomSmoothTime = 0.3f;

    private float zoomVelocity = 0.0f;
    private float targetZoom;

    // Rotation
    [Header("Rotation")]
    [SerializeField]
    private float rotationSensitivity;

    [SerializeField]
    private float rotationSmoothTime = 0.1f;

    private float rotationVelocity = 0.0f;
    private float targetRotation;

    private void Start() {
        targetZoom = zoomRange.y;
        targetRotation = target.localEulerAngles.y;
    }

    void SetState(STATE state) {

        if (this.state == state) {
            return;
        }

        this.state = state;

    }

    private void Update() {

        if (Input.GetMouseButtonDown(1)) {
            SetState(STATE.DRAGGING);
            lastCursorPos = Input.mousePosition;
        }

        if (state == STATE.DRAGGING && Input.GetMouseButtonUp(1)) {
            SetState(STATE.NONE);
        }

        ApplyMoveTarget();
        ApplyZoom();
        ApplyRotationTarget();

        lastCursorPos = Input.mousePosition;

    }

    private void ApplyMoveTarget() {

        if (state == STATE.DRAGGING) {
            ApplyMoveTargetDragging();
            return;
        }

    }

    private void ApplyMoveTargetDragging() {

        bool recalculateWorldPositions = Input.mousePosition != lastCursorPos;

        if (recalculateWorldPositions) {
            Vector3 lastPosWorld = ScreenToWorldPointFloor(lastCursorPos);
            Vector3 posWorld = ScreenToWorldPointFloor(Input.mousePosition);
            var baseMove = lastPosWorld - posWorld;
            target.position += baseMove;
        }

    }

    private void ApplyZoom() {

        float scrollDelta = Input.mouseScrollDelta.y;

        float scroll = scrollDelta * zoomSensibility;
        targetZoom = Mathf.Clamp(targetZoom + scroll, zoomRange.y, zoomRange.x);

        CinemachineOrbitalTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        // Go to target zoom
        float value = Mathf.SmoothDamp(transposer.m_FollowOffset.z, targetZoom, ref zoomVelocity, zoomSmoothTime);

        transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, value);
   
    }

    private void ApplyRotationTarget() {

        var axis = 0f;

        if (Input.GetKeyDown(KeyCode.A)) {
            axis = 1f * 90f;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            axis = -1f * 90f;
        }

        //targetRotation = targetRotation + axis * (rotationSensitivity * Time.deltaTime);
        //float value = Mathf.SmoothDamp(target.localEulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

        //target.localEulerAngles = new Vector3(target.localEulerAngles.x, value, target.localEulerAngles.z);

        target.localEulerAngles = new Vector3(target.localEulerAngles.x, target.localEulerAngles.y + axis, target.localEulerAngles.z);
    }


    private Vector3 ScreenToWorldPointFloor(Vector3 screenPos) {

        Ray ray = sourceCamera.ScreenPointToRay(screenPos);

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        plane.Raycast(ray, out float distance);
        Vector3 pos = ray.GetPoint(distance);
        pos.y = 0f;

        return pos;

    }

}

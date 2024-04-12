using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using UnityEngine.UIElements;
using Unity.VisualScripting.FullSerializer;

public class CameraControlerIso : MonoBehaviour
{
    [SerializeField]
    private Camera sourceCamera;

    [SerializeField]
    private Transform target;
    public Transform Target => target;

    private enum STATE { NONE, DRAGGING }

    // Current camera behaviour
    private STATE state;

    // Move
    private Vector3 lastCursorPos;
    private Vector3 cursorPos;
    private Vector3? lastPosWorld;
    private Vector3? posWorld;

    // Zoom
    [Header("Zoom")]
    [SerializeField]
    private CinemachineCameraOffset cameraOffset;

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

    private void Start() {
        targetZoom = zoomRange.y;
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

        cursorPos = Input.mousePosition;

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

        bool recalculateWorldPositions = cursorPos != lastCursorPos;

        if (recalculateWorldPositions) {

            lastPosWorld = ScreenToWorldPointFloor(lastCursorPos);
            posWorld = ScreenToWorldPointFloor(cursorPos);
            var baseMove = lastPosWorld.Value - posWorld.Value;
            target.position += baseMove;
        }
    }

    private void ApplyZoom() {

        float scrollDelta = Input.mouseScrollDelta.y;

        Debug.Log(Input.mouseScrollDelta.y);

        float scroll = scrollDelta * zoomSensibility;
        targetZoom = Mathf.Clamp(targetZoom + scroll, zoomRange.y, zoomRange.x);

        // Go to target zoom
        float value = Mathf.SmoothDamp(cameraOffset.m_Offset.z, targetZoom, ref zoomVelocity, zoomSmoothTime);

        cameraOffset.m_Offset = new Vector3(cameraOffset.m_Offset.x, cameraOffset.m_Offset.y, value);
   
    }

    private void ApplyRotationTarget() {

        var axis = 0f;

        if (Input.GetKey(KeyCode.A)) {
            axis = 1f;
        }

        if (Input.GetKey(KeyCode.E)) {
            axis = -1f;
        }

        
        target.localEulerAngles = new Vector3(target.localEulerAngles.x, target.localEulerAngles.y + (axis * rotationSensitivity * Time.deltaTime), target.localEulerAngles.z);
    }


    private Vector3 ScreenToWorldPointFloor(Vector3 screenPos) {

        Ray ray = sourceCamera.ScreenPointToRay(screenPos);
        float angle = Vector3.Angle(ray.direction, Vector3.down);
        float distance = ray.origin.y / Mathf.Cos(angle * Mathf.Deg2Rad);
        Vector3 pos = ray.GetPoint(distance);
        pos.y = 0f;
        return pos;

    }

}

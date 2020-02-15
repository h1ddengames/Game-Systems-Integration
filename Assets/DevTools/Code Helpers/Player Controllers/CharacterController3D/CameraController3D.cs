using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraController3D : MonoBehaviour
{
    [BoxGroup("Target"), Tooltip("Where should the camera look?"), SerializeField]
    private Transform target;

    [BoxGroup("Cursor"), Tooltip("Should the camera move when player input is received?"), SerializeField]
    private bool allowCameraMovement = false;
    [BoxGroup("Cursor"), Tooltip("Should the cursor stay locked within the game window?"), SerializeField]
    private bool lockCursor = true;
    [BoxGroup("Cursor"), Tooltip("How fast should the camera move when the user moves their mouse?"), SerializeField]
    private float mouseSensitivity = 10;

    [BoxGroup("Camera Settings"), Tooltip("How fast should the camera zoom in and out?"), SerializeField]
    private float cameraZoomSpeed = 16;
    [BoxGroup("Camera Settings"), Tooltip("How far should the character be able to see in the Y axis (up and down)?"), SerializeField]
    private Vector2 pitchMinMax = new Vector2(-40, 85);
    [BoxGroup("Camera Settings"), Tooltip("How much smoothing should be applied when the camera moves?"), SerializeField]
    private float rotationSmoothTime = 0.12f;

    [BoxGroup("Distance From Target"), Tooltip("How far should the user be able to move the camera from the player?"), SerializeField]
    private Vector2 distanceFromTargetMinMax = new Vector2(1, 6);
    [BoxGroup("Distance From Target"), Tooltip("How far from the target should the camera be?"), SerializeField]
    private float distanceFromTarget = 3;

    [SerializeField] GameObject captureCursorPanel;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject itemMenu;
    [SerializeField] GameObject equipmentMenu;
    [SerializeField] GameObject skillsMenu;
    [SerializeField] GameObject statsMenu;

    Vector3 _rotationSmoothVelocity;
    Vector3 _currentRotation;

    // Rotation on the y-axis.
    float _yaw;
    // Rotation on the x-axis
    float _pitch;

    public Transform Target { get => target; set => target = value; }
    public bool AllowCameraMovement { get => allowCameraMovement; set => allowCameraMovement = value; }
    public bool LockCursor { get => lockCursor; set => lockCursor = value; }
    public float MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }
    public float CameraZoomSpeed { get => cameraZoomSpeed; set => cameraZoomSpeed = value; }
    public Vector2 PitchMinMax { get => pitchMinMax; set => pitchMinMax = value; }
    public float RotationSmoothTime { get => rotationSmoothTime; set => rotationSmoothTime = value; }
    public Vector2 DistanceFromTargetMinMax { get => distanceFromTargetMinMax; set => distanceFromTargetMinMax = value; }
    public float DistanceFromTarget { get => distanceFromTarget; set => distanceFromTarget = value; }
    public GameObject CaptureCursorPanel { get => captureCursorPanel; set => captureCursorPanel = value; }
    public GameObject SettingsMenu { get => settingsMenu; set => settingsMenu = value; }
    public GameObject ItemMenu { get => itemMenu; set => itemMenu = value; }
    public GameObject EquipmentMenu { get => equipmentMenu; set => equipmentMenu = value; }
    public GameObject SkillsMenu { get => skillsMenu; set => skillsMenu = value; }
    public GameObject StatsMenu { get => statsMenu; set => statsMenu = value; }

    private void Start() {
        if(LockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate() {
        if (Target == null) {
            Debug.LogWarning("Please set the target to the Target GameObject on your player.");
            return;
        }

        CursorState();

        // Zoom the camera close to the target if the player scrolls their mouse up, further if the mouse scrolls down.
        DistanceFromTarget -= Input.mouseScrollDelta.y * Time.deltaTime * CameraZoomSpeed;
        DistanceFromTarget = Mathf.Clamp(DistanceFromTarget, DistanceFromTargetMinMax.x, DistanceFromTargetMinMax.y);

        _yaw += Input.GetAxis("Mouse X");
        _pitch -= Input.GetAxis("Mouse Y");
        _pitch = Mathf.Clamp(_pitch, PitchMinMax.x, PitchMinMax.y);
        
        if (!AllowCameraMovement) {
            _currentRotation = Vector3.SmoothDamp(_currentRotation, new Vector3(_pitch, _yaw), ref _rotationSmoothVelocity, RotationSmoothTime);
            transform.eulerAngles = _currentRotation;
        }

        transform.position = Target.position - transform.forward * DistanceFromTarget;
    }

    public void CursorState() {
        if (LockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CaptureCursorPanel.SetActive(true);
        }
    }

    public void ClickedCaptureCursorPanel() {
        LockCursor = !LockCursor;
        CursorState();
    }
}

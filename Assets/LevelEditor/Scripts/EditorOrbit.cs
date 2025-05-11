using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EditorOrbit : MonoBehaviour
{
    [Tooltip("Default Camera distance from the pivot point.")]
    public float DefaultDistance = 25.0f;

    [Tooltip("Mouse Orbit Dragging Sensitivity.")]
    public float OrbitSensitivity = 3.0f;

    [Tooltip("Mouse Pivot Point Dragging Sensitivity.")]
    public float PivotSensitivity = 1.0f;

    [Tooltip("Mouse Scroll/Zoom Sensitivity.")]
    public float ZoomSensitivity = 1.0f;

    [Tooltip("Camera Smooth Factor. Lower = more smooth.")]
    public float SmoothFactor = 0.025f;

    [Tooltip("Player Prefab.")]
    public GameObject PlayerPrefab;

    // Current camera pivot point
    private Vector3 pivotPoint;

    private Vector2 orbitVelocity = Vector2.zero; // Current orbit velocity
    private Vector2 pivotVelocity = Vector2.zero; // Current pivot velocity

    // EditorSelector component
    private EditorSelector selector;

    void Start()
    {
        selector = GetComponent<EditorSelector>();

        // Set the pivot point and position the camera
        pivotPoint = transform.position;
        transform.position = pivotPoint + Vector3.right * DefaultDistance;

        // Make the camera look at the pivot point
        transform.LookAt(pivotPoint);
    }

    void Update()
    {
        ProcessMovement();

        // DEBUG PLAY LOGIC
        // Press space to play
        if (Input.GetKeyDown("space"))
        {
            // Disable Editor Stuff
            gameObject.SetActive(false);
            Camera editorCamera = GetComponent<Camera>();
            editorCamera.enabled = false;

            // Clear all selection highlights
            selector.ClearAllHighlights();
            selector.SelectedVoxels.Clear();

            // Instantiate the player
            GameObject player = Instantiate(PlayerPrefab, new Vector3(0, 48, 0), Quaternion.identity);

            // Find the player's camera and set it as the main camera
            Camera playerCamera = player.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                playerCamera.enabled = true; // Ensure the player's camera is enabled
            }
        }
    }

    void ProcessMovement()
    {
        // Default LMB drag logic
        // Orbit around Pivot Point
        if (Input.GetMouseButton(0))
        {
            // Get mouse movement
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            // Invert vertical input to reverse orbit direction
            float vertical = -mouseDelta.y;

            // Update velocity with current mouse movement
            orbitVelocity = new Vector2(mouseDelta.x, vertical);

            // Rotate the camera around the pivot point
            transform.RotateAround(pivotPoint, Vector3.up, mouseDelta.x * OrbitSensitivity);
            transform.RotateAround(pivotPoint, transform.right, vertical * OrbitSensitivity);
        }
        else
        {
            // If mouse button is not held, smooth the camera's velocity
            orbitVelocity = Vector2.Lerp(orbitVelocity, Vector2.zero, SmoothFactor);

            transform.RotateAround(pivotPoint, Vector3.up, orbitVelocity.x * OrbitSensitivity);
            transform.RotateAround(pivotPoint, transform.right, orbitVelocity.y * OrbitSensitivity);
        }

        // Default RMB drag logic
        // Move pivot point
        if (Input.GetMouseButton(1))
        {
            // Get mouse movement
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            // Invert input to reverse drag direction
            float vertical = -mouseDelta.y;
            float horizontal = -mouseDelta.x;

            // Update velocity with current mouse movement
            pivotVelocity = new Vector2(horizontal, vertical);

            // Move the pivot point
            pivotPoint += transform.right * horizontal * PivotSensitivity;
            pivotPoint += transform.up * vertical * PivotSensitivity;

            // Apply the movement to the camera position
            transform.position += transform.right * horizontal * PivotSensitivity;
            transform.position += transform.up * vertical * PivotSensitivity;
        }
        else
        {
            // If mouse button is not held, smooth the camera's velocity
            pivotVelocity = Vector2.Lerp(pivotVelocity, Vector2.zero, SmoothFactor);

            // Move the pivot point
            pivotPoint += transform.right * pivotVelocity.x * PivotSensitivity;
            pivotPoint += transform.up * pivotVelocity.y * PivotSensitivity;

            // Apply the movement to the camera position
            transform.position += transform.right * pivotVelocity.x * PivotSensitivity;
            transform.position += transform.up * pivotVelocity.y * PivotSensitivity;
        }

        // Default scroll logic
        // Zoom in and out
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.position += transform.forward * ZoomSensitivity; // Zoom out
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.position -= transform.forward * ZoomSensitivity; // Zoom in
        }
    }
}
using UnityEngine;

public class EditorController : MonoBehaviour
{
    [Tooltip("Default Camera distance from the pivot point.")]
    public float DefaultDistance = 25.0f;
    
    [Tooltip("Mouse Orbit Dragging Sensitivity.")]
    public float OrbitSensitivity = 3.0f;

    [Tooltip("Mouse Pivot Point Dragging Sensitivity.")]
    public float PivotSensitivity = 1.0f;

    [Tooltip("Camera Smooth Factor. Lower = more smooth.")]
    public float SmoothFactor = 0.025f;

    // Current camera pivot point
    private Vector3 pivotPoint;

    // Current velocity
    private Vector2 currentVelocity = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the pivot point and position the camera
        pivotPoint = transform.position;
        transform.position = pivotPoint + Vector3.right * DefaultDistance;

        // Make the camera look at the pivot point
        transform.LookAt(pivotPoint);
    }

    // Update is called once per frame
    void Update()
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
            currentVelocity = new Vector2(mouseDelta.x, vertical);

            // Rotate the camera around the pivot point
            transform.RotateAround(pivotPoint, Vector3.up, mouseDelta.x * OrbitSensitivity);
            transform.RotateAround(pivotPoint, transform.right, vertical * OrbitSensitivity);
        }
        else
        {
            // If mouse button is not held, smooth the camera's velocity
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, SmoothFactor);

            transform.RotateAround(pivotPoint, Vector3.up, currentVelocity.x * OrbitSensitivity);
            transform.RotateAround(pivotPoint, transform.right, currentVelocity.y * OrbitSensitivity);
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

            // Move the pivot point
            pivotPoint += transform.right * horizontal * PivotSensitivity;
            pivotPoint += transform.up * vertical * PivotSensitivity;

            // Apply the movement to the camera position
            transform.position += transform.right * horizontal * PivotSensitivity;
            transform.position += transform.up * vertical * PivotSensitivity;
        }
    }
}

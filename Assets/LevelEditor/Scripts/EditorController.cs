using UnityEngine;

public class EditorController : MonoBehaviour
{
    [Tooltip("Default Camera distance from the pivot point.")]
    public float DefaultDistance = 25.0f;
    
    [Tooltip("Mouse Dragging Sensitivity.")]
    public float Sensitivity = 3.0f;

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
        // Default drag logic
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
            transform.RotateAround(pivotPoint, Vector3.up, mouseDelta.x * Sensitivity);
            transform.RotateAround(pivotPoint, transform.right, vertical * Sensitivity);
        }
        else
        {
            // If mouse button is not held, smooth the camera's velocity
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, SmoothFactor);

            transform.RotateAround(pivotPoint, Vector3.up, currentVelocity.x * Sensitivity);
            transform.RotateAround(pivotPoint, transform.right, currentVelocity.y * Sensitivity);
        }
    }
}

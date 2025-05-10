using UnityEngine;

public class EditorController : MonoBehaviour
{
    [Tooltip("Default camera distance from the pivot point.")]
    public float DefaultDistance = 25.0f;
    
    [Tooltip("Mouse Dragging Sensitivity.")]
    public float Sensitivity = 2.0f;

    // Current camera pivot point
    private Vector3 pivotPoint;

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

            // Invert mouse input to reverse the orbit direction
            float horizontal = -mouseDelta.x;
            float vertical = -mouseDelta.y;

            // Rotate the camera around the pivot point
            transform.RotateAround(pivotPoint, Vector3.up, horizontal * Sensitivity);
            transform.RotateAround(pivotPoint, transform.right, vertical * Sensitivity);
        }
    }
}

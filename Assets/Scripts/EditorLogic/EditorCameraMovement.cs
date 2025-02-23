using UnityEngine;

public class EditorCameraMovement : MonoBehaviour
{
    public int ZoomSpeedMod, MoveSpeedMod, RotSpeedMod;

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            MoveCloser();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            MoveFarther();
        }
        Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetMouseButton(1))
        {
            Rotate();
        }
    }
    void Move(float x, float y)
    {
        Vector3 movement = new Vector3(x, 0, y) * MoveSpeedMod * Time.deltaTime;
        transform.Translate(movement);
    }
    void MoveCloser()
    {
        transform.Translate(transform.forward * Time.deltaTime * ZoomSpeedMod, Space.World);
    }
    void MoveFarther()
    {
        transform.Translate(transform.forward * Time.deltaTime * -1 * ZoomSpeedMod, Space.World);
    }
    void Rotate()
    {
        transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * RotSpeedMod);
        transform.RotateAround(transform.position, transform.right, -Input.GetAxis("Mouse Y") * Time.deltaTime * RotSpeedMod);
    }
}

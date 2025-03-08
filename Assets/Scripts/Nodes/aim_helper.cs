using UnityEngine;

public class aim_helper : MonoBehaviour
{
    public Vector3 direction;
    public ShootLaser laserScript;
    public float radius;
    private void Update()
    {
        print(Vector3.Distance(transform.position, laserScript.bufPosition));
        if (Vector3.Distance(transform.position, laserScript.bufPosition) <= radius)
        {
            laserScript.bufPosition = transform.position;
            laserScript.bufDirection = direction;
        }
    }
}

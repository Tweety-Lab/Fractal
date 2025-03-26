using UnityEngine;

public class door_node_helpter : MonoBehaviour
{
    public door_node parentNode;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            parentNode.Active = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        parentNode.Active = false;
    }
}

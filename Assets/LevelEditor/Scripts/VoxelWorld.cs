using UnityEngine;

/// <summary>
/// A Voxel World used for level editing.
/// </summary>
public class VoxelWorld : MonoBehaviour
{
    [Header("Voxel Settings")]
    [Tooltip("The size of each voxel.")]
    public float VoxelSize = 1.0f;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}

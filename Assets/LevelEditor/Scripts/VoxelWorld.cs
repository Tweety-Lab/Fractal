using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Voxel World used for level editing.
/// </summary>
public class VoxelWorld : MonoBehaviour
{
    [Header("Voxel Settings")]
    [Tooltip("The size of each voxel.")]
    public float VoxelSize = 1.0f;

    // Voxel World
    private Dictionary<Vector3Int, Voxel> voxelWorldData = new Dictionary<Vector3Int, Voxel>();

    /// <summary>
    /// Get a voxel from it's position.
    /// </summary>
    public Voxel GetVoxel(Vector3Int position) => voxelWorldData[position];

    /// <summary>
    /// Add a voxel to the world.
    /// </summary>
    public void AddVoxel(Vector3Int position, Voxel voxel) => voxelWorldData[position] = voxel;

    /// <summary>
    /// Remove a voxel from the world.
    /// </summary>
    public void RemoveVoxel(Vector3Int position) => voxelWorldData.Remove(position);

    // Update the voxel world (render it, etc)
    public void UpdateVoxelWorld()
    {
        foreach (var voxel in voxelWorldData.Values)
        {
            voxel.UpdateVoxel();
        }
    }

    private void OnEnable()
    {
        UpdateVoxelWorld();
    }

    private void OnDisable()
    {
        
    }
}

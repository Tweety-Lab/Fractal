using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Voxel World used for level editing.
/// </summary>
public class VoxelWorld : MonoBehaviour
{
    [Header("Voxel Settings")]
    [Tooltip("The size of each voxel.")]
    public int VoxelSize = 1;

    // Voxel World
    private Dictionary<Vector3Int, Voxel> voxelWorldData = new Dictionary<Vector3Int, Voxel>();

    // All currently rendered voxel GameObjects
    private Dictionary<Vector3Int, GameObject> renderedVoxels = new Dictionary<Vector3Int, GameObject>();

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
        // Remove GameObjects that no longer have voxels
        List<Vector3Int> toRemove = new List<Vector3Int>();
        foreach (var kvp in renderedVoxels)
        {
            if (!voxelWorldData.ContainsKey(kvp.Key))
            {
                GameObject.Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var key in toRemove)
        {
            renderedVoxels.Remove(key);
        }

        // Add new voxels
        foreach (var voxelData in voxelWorldData)
        {
            Voxel voxel = voxelData.Value;
            Vector3Int position = voxelData.Key;

            // Create the voxels gameobject representation
            voxel.GameObject = new GameObject("Voxel");
            voxel.GameObject.transform.parent = transform;

            // Set size
            voxel.GameObject.transform.localScale = Vector3.one * VoxelSize;

            // Set position
            voxel.GameObject.transform.position = position * VoxelSize;

            // Give it a mesh
            if (voxel.Type != VoxelType.Terrain)
                continue;

            voxel.GameObject.AddComponent<MeshFilter>();
            voxel.GameObject.GetComponent<MeshFilter>().mesh = VoxelUtility.VoxelMesh;

            voxel.GameObject.AddComponent<MeshRenderer>();
            voxel.GameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        }
    }

    void Start()
    {
        // Setup test voxels
        // 3X3 Square
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    AddVoxel(new Vector3Int(x, y, z), new Voxel());
                }
            }
        }

        // Update voxel world
        UpdateVoxelWorld();
    }
}

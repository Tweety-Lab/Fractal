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

    [Header("Materials")]
    public List<MaterialWeight> FloorMaterials = new List<MaterialWeight>();
    public List<MaterialWeight> WallMaterials = new List<MaterialWeight>();

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

    /// <summary>
    /// Get the voxel position from a world position.
    /// </summary>
    public Vector3Int WorldToVoxelPosition(Vector3 worldPosition) => new Vector3Int((int)(worldPosition.x / VoxelSize), (int)(worldPosition.y / VoxelSize), (int)(worldPosition.z / VoxelSize));

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
            renderedVoxels.Remove(key);

        // Add new voxels
        foreach (var voxelData in voxelWorldData)
        {
            Voxel voxel = voxelData.Value;
            Vector3Int position = voxelData.Key;

            // Skip if already rendered
            if (renderedVoxels.ContainsKey(position))
                continue;

            // Create the Voxels gameobject representation
            GameObject voxelGO = new GameObject("Voxel");
            voxelGO.transform.parent = transform;
            voxelGO.transform.localScale = Vector3.one * VoxelSize;
            voxelGO.transform.position = position * VoxelSize;

            // Add mesh
            if (voxel.Type == VoxelType.Terrain)
            {
                var meshFilter = voxelGO.AddComponent<MeshFilter>();
                meshFilter.mesh = VoxelUtility.VoxelMesh;

                var meshRenderer = voxelGO.AddComponent<MeshRenderer>();

                // Add Random materials depending on side
                meshRenderer.materials = new Material[] {
                    // 4 Walls
                    MaterialWeight.GetRandomMaterial(WallMaterials),
                    MaterialWeight.GetRandomMaterial(WallMaterials),
                    MaterialWeight.GetRandomMaterial(WallMaterials),
                    MaterialWeight.GetRandomMaterial(WallMaterials),

                    // Roof + Floor
                    MaterialWeight.GetRandomMaterial(FloorMaterials),
                    MaterialWeight.GetRandomMaterial(FloorMaterials)
                };
            }

            // Store Voxel
            voxel.GameObject = voxelGO;
            renderedVoxels[position] = voxelGO;
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

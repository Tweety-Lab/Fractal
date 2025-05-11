using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class EditorLevel : MonoBehaviour
{
    // Voxel World
    Dictionary<Vector3Int, Voxel> voxelWorld = new();

    // Store voxels that have been processed
    HashSet<Vector3Int> processedVoxels = new();

    // Pre-built Voxel Mesh
    static Mesh voxelMesh;

    [Tooltip("Size of each Voxel.")]
    public float VoxelSize = 2f;

    [Tooltip("Default Material for Voxel Floors.")]
    public Material DefaultFloorMaterial;

    [Tooltip("Default Material for Voxel Walls.")]
    public Material DefaultWallMaterial;

    void OnEnable()
    {
        // Test voxel room
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int z = 0; z < 6; z++)
                {
                    // Check if the voxel is on the outer edges (walls, floor, or ceiling)
                    if (x == 0 || x == 5 || y == 0 || y == 5 || z == 0 || z == 5)
                    {
                        voxelWorld[new Vector3Int(x, y, z)] = new Voxel();
                    }
                }
            }
        }

        ProcessVoxels();
    }

    void ProcessVoxels()
    {
        foreach (var voxel in voxelWorld)
        {
            // Skip voxels that have already been processed
            if (processedVoxels.Contains(voxel.Key))
                continue;

            // Initialize voxelMesh
            if (voxelMesh == null)
                voxelMesh = VoxelUtility.CreateVoxelCube();

            // Create the voxel
            GameObject voxelObject = new GameObject("Voxel");
            voxelObject.transform.parent = transform;

            // Add MeshFilter and MeshRenderer components
            MeshFilter meshFilter = voxelObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = voxelObject.AddComponent<MeshRenderer>();
            MeshCollider meshCollider = voxelObject.AddComponent<MeshCollider>();

            // Use the pre-created voxel mesh for all voxels
            meshFilter.mesh = voxelMesh;
            meshCollider.sharedMesh = voxelMesh;

            // Set scale
            voxelObject.transform.localScale = Vector3.one * VoxelSize;

            // Position the voxel
            voxelObject.transform.position = Vector3.Scale(voxel.Key, Vector3.one * VoxelSize);

            // Material array for different faces
            Material[] faceMaterials = new Material[6];

            // Apply materials to each face based on what type it is
            for (int i = 0; i < 6; i++)
            {
                // Get the direction this face is pointing based on its position in the cube
                Vector3 direction = Vector3.zero;
                switch (i)
                {
                    case 0: direction = Vector3.forward; break;  // Front face
                    case 1: direction = Vector3.back; break;    // Back face
                    case 2: direction = Vector3.left; break;    // Left face
                    case 3: direction = Vector3.right; break;   // Right face
                    case 4: direction = Vector3.up; break;      // Top face
                    case 5: direction = Vector3.down; break;    // Bottom face
                }

                // Direction Up = floor
                // Other Directions = wall
                if (direction == Vector3.up)
                {
                    faceMaterials[i] = DefaultFloorMaterial;
                }
                else
                {
                    faceMaterials[i] = DefaultWallMaterial;
                }
            }

            // Assign materials
            meshRenderer.materials = faceMaterials;
            

            // Add Voxel to processed Voxels
            processedVoxels.Add(voxel.Key);
        }
    }
}

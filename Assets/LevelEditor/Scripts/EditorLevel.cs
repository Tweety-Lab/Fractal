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
    public float VoxelSize = 16f;

    [Tooltip("Default Material for Voxel Floors.")]
    public Material DefaultFloorMaterial;

    [Tooltip("Default Material for Voxel Walls.")]
    public Material DefaultWallMaterial;


    void Start()
    {
        // Create Test voxel at 0,0,0
        voxelWorld[new Vector3Int(0, 0, 0)] = new Voxel();

        // Create Test Voxel at 1,0,0
        voxelWorld[new Vector3Int(1, 0, 0)] = new Voxel();

        ProcessVoxels();
    }

    void ProcessVoxels()
    {
        foreach (var voxel in voxelWorld)
        {
            // Skip voxels that have already been processed
            if (processedVoxels.Contains(voxel.Key))
                return;

            // Initialize voxelMesh
            if (voxelMesh == null)
                voxelMesh = VoxelUtility.CreateVoxelCube();

            // Create the voxel
            GameObject voxelObject = new GameObject("Voxel");

            // Add MeshFilter and MeshRenderer components
            MeshFilter meshFilter = voxelObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = voxelObject.AddComponent<MeshRenderer>();

            // Use the pre-created voxel mesh for all voxels
            meshFilter.mesh = voxelMesh;

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



    void Update()
    {
        
    }
}

using System.Collections.Generic;
using UnityEngine;

public class EditorLevel : MonoBehaviour
{
    // Voxel World
    Dictionary<Vector3Int, Voxel> voxelWorld = new();

    [Tooltip("Size of each Voxel.")]
    public float VoxelSize = 16f;

    [Tooltip("Default Material for Voxels.")]
    public Material DefaultMaterial;


    void Start()
    {
        // Create Test voxel at 0,0,0
        voxelWorld[new Vector3Int(0, 0, 0)] = new Voxel();

        // Create Test Voxel at 1,0,0
        voxelWorld[new Vector3Int(1, 0, 0)] = new Voxel();

        // Render Voxel World
        foreach (var voxel in voxelWorld)
        {
            // Create the voxel
            GameObject voxelObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Set scale
            voxelObject.transform.localScale = Vector3.one * VoxelSize;

            // Position the voxel
            voxelObject.transform.position = voxel.Key;

            // If the Voxel defines a Material, apply it
            // Otherwise apply default
            if (voxel.Value.Material != null)
            {
                voxelObject.GetComponent<MeshRenderer>().material = voxel.Value.Material;
            } else
            {
                voxelObject.GetComponent<MeshRenderer>().material = DefaultMaterial;
            }
        }
    }

    void Update()
    {
        
    }
}

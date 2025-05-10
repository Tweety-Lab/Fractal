using System.Collections.Generic;
using UnityEngine;

public class EditorLevel : MonoBehaviour
{
    // Voxel World
    Dictionary<Vector3Int, Voxel> voxelWorld = new();

    [Tooltip("Size of each Voxel.")]
    public float voxelSize = 16f;


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
            voxelObject.transform.localScale = Vector3.one * voxelSize;

            // Position the voxel
            voxelObject.transform.position = voxel.Key;
        }
    }

    void Update()
    {
        
    }
}

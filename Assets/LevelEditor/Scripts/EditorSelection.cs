using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EditorSelection : MonoBehaviour
{
    // Positions of all selected voxels
    private List<Vector3Int> selectedVoxels = new();

    // Update is called once per frame
    void Update()
    {
        // LMB
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Dont continue if we didnt hit a voxel
                if (hit.collider.transform.GetComponentInParent<VoxelWorld>() == null)
                    return;

                // Get the Voxel we hit
                VoxelWorld voxelWorld = hit.collider.transform.GetComponentInParent<VoxelWorld>();
                Voxel voxel = voxelWorld.GetVoxel(voxelWorld.WorldToVoxelPosition(hit.point));

                // Voxel is part of the chamber
                if (voxel.Type == VoxelType.Terrain)
                {
                    selectedVoxels.Add(voxelWorld.WorldToVoxelPosition(hit.point));
                    Debug.Log("Selected: " + voxelWorld.WorldToVoxelPosition(hit.point));
                }
            }
        }
    }
}

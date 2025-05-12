using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EditorSelection : MonoBehaviour
{
    // Positions of all selected voxels
    private List<Vector3Int> selectedVoxels = new();
    private VoxelWorld voxelWorld;

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

                // Optionally Clear selected voxels
                if (!Input.GetKey(KeyCode.LeftShift) || !Input.GetKey(KeyCode.RightShift))
                    selectedVoxels.Clear();

                // Get the Voxel we hit
                voxelWorld = hit.collider.transform.GetComponentInParent<VoxelWorld>();
                Voxel voxel = voxelWorld.GetVoxel(voxelWorld.WorldToVoxelPosition(hit.collider.transform.position));

                // Voxel is part of the chamber
                if (voxel.Type == VoxelType.Terrain)
                {
                    selectedVoxels.Add(voxelWorld.WorldToVoxelPosition(hit.collider.transform.position));
                    Debug.Log("Selected: " + voxelWorld.WorldToVoxelPosition(hit.collider.transform.position));
                }
            }
        }

        // Voxel pushing/pulling
        // + Key
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Equals))
        {
            // List to store positions to add and remove after the loop
            List<Vector3Int> positionsToAdd = new List<Vector3Int>();
            List<Vector3Int> positionsToRemove = new List<Vector3Int>();

            foreach (Vector3Int position in selectedVoxels)
            {
                // Add voxel above
                Vector3Int newPosition = position + new Vector3Int(0, 1, 0);
                voxelWorld.AddVoxel(newPosition, voxelWorld.GetVoxel(position));
                voxelWorld.UpdateVoxelWorld();

                // Collect the positions to update after the loop
                positionsToAdd.Add(newPosition);
                positionsToRemove.Add(position);
            }

            // Update the selectedVoxels list outside the loop
            foreach (Vector3Int position in positionsToAdd)
                selectedVoxels.Add(position);

            foreach (Vector3Int position in positionsToRemove)
                selectedVoxels.Remove(position);
        }
    }
}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EditorSelection : MonoBehaviour
{
    // POSITION        |      NORMAL
    private Dictionary<Vector3Int, Vector3Int> selectedVoxels = new();

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
                voxelWorld = hit.collider.transform.GetComponentInParent<VoxelWorld>();
                if (voxelWorld == null) return;

                Vector3Int voxelPos = voxelWorld.WorldToVoxelPosition(hit.collider.transform.position);
                Voxel voxel = voxelWorld.GetVoxel(voxelPos);

                // Dont continue if we hit an object voxel-type
                if (voxel.Type != VoxelType.Terrain) return;

                // Clear selection unless shift is held
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                    selectedVoxels.Clear();

                Vector3Int normal = Vector3Int.RoundToInt(hit.normal);
                selectedVoxels[voxelPos] = normal;
            }
        }

        // Voxel pushing/pulling
        // + Key
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Equals))
        {
            Dictionary<Vector3Int, Vector3Int> updatedSelection = new();

            foreach (var kvp in selectedVoxels)
            {
                Vector3Int currentPos = kvp.Key;
                Vector3Int normal = kvp.Value;

                Vector3Int targetPos = currentPos - normal * -1;

                voxelWorld.AddVoxel(targetPos, voxelWorld.GetVoxel(currentPos));
                updatedSelection[targetPos] = normal;
            }

            voxelWorld.UpdateVoxelWorld();
            selectedVoxels = updatedSelection;
        }

        // - Key
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Dictionary<Vector3Int, Vector3Int> updatedSelection = new();

            foreach (var kvp in selectedVoxels)
            {
                Vector3Int currentPos = kvp.Key;
                Vector3Int normal = kvp.Value;

                Vector3Int targetPos = currentPos + normal * -1;

                voxelWorld.AddVoxel(targetPos, voxelWorld.GetVoxel(currentPos));
                voxelWorld.RemoveVoxel(currentPos);
                updatedSelection[targetPos] = normal;
            }

            voxelWorld.UpdateVoxelWorld();
            selectedVoxels = updatedSelection;
        }
    }
}

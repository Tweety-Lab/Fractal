using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EditorSelection : MonoBehaviour
{
    [Header("Selection")]
    [Tooltip("The color to highlight selected voxels with.")]
    public Color SelectionColor = Color.yellow;

    // POSITION        |      NORMAL
    private Dictionary<Vector3Int, Vector3Int> selectedVoxels = new();

    private VoxelWorld voxelWorld;

    // Update is called once per frame
    void Update()
    {
        // LMB click to select
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Don't continue if we didn't hit a voxel
                voxelWorld = hit.collider.transform.GetComponentInParent<VoxelWorld>();
                if (voxelWorld == null) return;

                Vector3Int voxelPos = voxelWorld.WorldToVoxelPosition(hit.collider.transform.position);
                Voxel voxel = voxelWorld.GetVoxel(voxelPos);

                // Don't continue if we hit a non-terrain voxel
                if (voxel.Type != VoxelType.Terrain)
                    return;

                // Clear selection unless shift is held
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                    selectedVoxels.Clear();

                Vector3Int normal = Vector3Int.RoundToInt(hit.normal);

                // Add the voxel and its normal to the selection
                selectedVoxels[voxelPos] = normal;
            }
        }

        // Voxel pushing (shift + +)
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Equals))
        {
            // Update the selection to reflect the new positions after pulling
            Dictionary<Vector3Int, Vector3Int> updatedSelection = new();

            foreach (var kvp in selectedVoxels)
            {
                // Only run pushing logic for terrain
                if (!voxelWorld.TryGetVoxel(kvp.Key, out Voxel checkVoxel) || checkVoxel.Type != VoxelType.Terrain)
                    return;

                Vector3Int currentPos = kvp.Key;
                Vector3Int normal = kvp.Value;

                // Check if there's an object voxel in the way
                if (voxelWorld.TryGetVoxel(currentPos + normal, out Voxel voxel) && voxel.Type == VoxelType.Object)
                {
                    // Push the object voxel
                    Vector3Int desiredObjectPos = currentPos + normal * 2;
                    Vector3Int objectPos = currentPos + normal;

                    voxelWorld.MoveVoxel(objectPos, desiredObjectPos);
                    voxelWorld.UpdateVoxelWorld();
                }

                // Move the selected voxel in the direction opposite to its normal
                Vector3Int targetPos = currentPos + normal;

                voxelWorld.AddVoxel(targetPos, voxelWorld.GetVoxel(currentPos));
                updatedSelection[targetPos] = normal;
            }

            voxelWorld.UpdateVoxelWorld();
            selectedVoxels = updatedSelection;
        }

        // Voxel pulling (shift + -)
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            // Update the selection to reflect the new positions after pushing
            Dictionary<Vector3Int, Vector3Int> updatedSelection = new();

            foreach (var kvp in selectedVoxels)
            {
                Vector3Int currentPos = kvp.Key;
                Vector3Int normal = kvp.Value;

                // Move the selected voxel in the direction opposite to its normal
                Vector3Int targetPos = currentPos - normal;

                voxelWorld.RemoveVoxel(currentPos);

                updatedSelection[targetPos] = normal;
            }

            voxelWorld.UpdateVoxelWorld();
            selectedVoxels = updatedSelection;
        }
    }
}

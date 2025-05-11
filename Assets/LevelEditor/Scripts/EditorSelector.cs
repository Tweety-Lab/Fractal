using System.Collections.Generic;
using UnityEngine;

public class VoxelSelection
{
    public GameObject voxel;
    public Vector3 normal;
    public int faceIndex; // Store the face index for highlighting

    public VoxelSelection(GameObject voxel, Vector3 normal, int faceIndex = 0)
    {
        this.voxel = voxel;
        this.normal = normal;
        this.faceIndex = faceIndex;
    }
}
public class EditorSelector : MonoBehaviour
{
    [Header("Selection Settings")]
    [Tooltip("Highlight Color of selected voxels.")]
    public Color SelectionHighlightColor = Color.yellow;

    // All currently selected voxels
    public List<VoxelSelection> SelectedVoxels { get; set; } = new List<VoxelSelection>();

    private void Update()
    {
        ProcessSelection();
        ProcessVoxelManipulation();
    }

    // Apply a highlight to a VoxelSelection
    public void ApplyHighlight(VoxelSelection selection)
    {
        if (selection != null && selection.voxel != null)
        {
            MeshRenderer renderer = selection.voxel.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material[] mats = renderer.materials;

                // Make sure the face index is valid
                int faceIndex = Mathf.Clamp(selection.faceIndex, 0, mats.Length - 1);

                // Apply highlight color to the specific face
                mats[faceIndex].color = SelectionHighlightColor;

                renderer.materials = mats;
            }
        }
    }

    // Clear all VoxelSelection highlights
    public void ClearAllHighlights()
    {
        foreach (var sel in SelectedVoxels)
        {
            if (sel.voxel != null)
            {
                MeshRenderer renderer = sel.voxel.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    Material[] mats = renderer.materials;
                    for (int i = 0; i < mats.Length; i++)
                    {
                        mats[i].color = Color.white;
                    }
                    renderer.materials = mats;
                }
            }
        }
    }

    void ProcessSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject.name == "Voxel")
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        // Clear previous selection and highlights
                        ClearAllHighlights();
                        SelectedVoxels.Clear();
                    }

                    // Determine face index from triangle index
                    int faceIndex = hit.triangleIndex / 2;
                    faceIndex = Mathf.Clamp(faceIndex, 0, 5); // Ensure valid range

                    SelectedVoxels.Add(new VoxelSelection(
                        hit.collider.gameObject,
                        hit.normal,
                        faceIndex
                    ));

                    // Apply highlights to new selection
                    ApplyHighlight(SelectedVoxels[SelectedVoxels.Count - 1]);
                }
            }
        }
    }

    void ProcessVoxelManipulation()
    {
        // Voxel Creation (+ Key)
        if (Input.GetKeyDown(KeyCode.Equals) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            var originalSelection = new List<VoxelSelection>(SelectedVoxels);

            // Clear current selections and highlights
            ClearAllHighlights();
            SelectedVoxels.Clear();

            foreach (var sel in originalSelection)
            {
                // Update Voxel World
                EditorLevel voxelWorld = sel.voxel.transform.parent.GetComponent<EditorLevel>();
                if (voxelWorld == null) return;

                Vector3 direction = sel.normal.normalized;
                float offset = sel.voxel.transform.localScale.x;
                Vector3 newPos = sel.voxel.transform.position + direction * offset;

                // Convert new position to grid coordinate
                Vector3Int gridPos = Vector3Int.RoundToInt(newPos / offset);

                // Check if voxel already exists
                if (!voxelWorld.VoxelWorld.ContainsKey(gridPos))
                {
                    // Add to VoxelWorld
                    voxelWorld.VoxelWorld[gridPos] = new Voxel();

                    // Dirty this voxel
                    voxelWorld.ProcessedVoxels.Remove(gridPos);

                    // Instantiate the voxel manually or call ProcessVoxels
                    voxelWorld.ProcessVoxels();

                    // Select and highlight it
                    GameObject voxel = voxelWorld.GetVoxelGameObject(gridPos);
                    var newSelection = new VoxelSelection(voxel, sel.normal, sel.faceIndex);
                    SelectedVoxels.Add(newSelection);
                    ApplyHighlight(newSelection);
                }
            }
        }

        // Voxel Destruction (- Key)
        if (Input.GetKeyDown(KeyCode.Minus) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            var originalSelection = new List<VoxelSelection>(SelectedVoxels);

            // Clear current selections and highlights
            ClearAllHighlights();
            SelectedVoxels.Clear();

            foreach (var sel in originalSelection)
            {
                // Update Voxel World
                EditorLevel voxelWorld = sel.voxel.transform.parent.GetComponent<EditorLevel>();
                if (voxelWorld == null) return;

                Vector3 voxelPos = sel.voxel.transform.position;
                float offset = sel.voxel.transform.localScale.x;
                Vector3Int gridPos = Vector3Int.RoundToInt(voxelPos / offset);

                // Remove from VoxelWorld
                if (voxelWorld.VoxelWorld.ContainsKey(gridPos))
                {
                    voxelWorld.VoxelWorld.Remove(gridPos);
                }

                // Determine the direction to look for the voxel behind
                Vector3 oppositeDirection = -sel.normal; // Inverse of the normal

                // Find the position of the voxel behind the destroyed voxel
                Vector3 behindVoxelPosition = voxelPos + oppositeDirection * offset;

                // Convert position to grid coordinates
                Vector3Int behindGridPos = Vector3Int.RoundToInt(behindVoxelPosition / offset);

                // Check if the voxel behind exists in the VoxelWorld
                if (voxelWorld.VoxelWorld.ContainsKey(behindGridPos))
                {
                    // Get the GameObject of the voxel behind
                    GameObject behindVoxelObject = voxelWorld.GetVoxelGameObject(behindGridPos);

                    if (behindVoxelObject != null)
                    {
                        // Add the behind voxel to the selection
                        SelectedVoxels.Add(new VoxelSelection(behindVoxelObject, sel.normal));

                        // Apply highlight to the selected voxel
                        ApplyHighlight(SelectedVoxels[^1]);
                    }
                }

                // Destroy the voxel after handling the voxel behind
                Destroy(sel.voxel);
            }
        }
    }
    int GetFaceIndexForNormal(Vector3 normal)
    {
        // Determine which face the normal corresponds to
        if (normal == Vector3.forward) return 0;  // Front face
        if (normal == Vector3.back) return 1;     // Back face
        if (normal == Vector3.left) return 2;     // Left face
        if (normal == Vector3.right) return 3;    // Right face
        if (normal == Vector3.up) return 4;       // Top face
        if (normal == Vector3.down) return 5;     // Bottom face

        return -1; // Default in case no match is found (shouldn't happen)
    }
}

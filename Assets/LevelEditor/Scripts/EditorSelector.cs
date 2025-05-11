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
                Vector3 direction = sel.normal.normalized;
                float offset = sel.voxel.transform.localScale.x; // Assuming uniform scale
                Vector3 newPos = sel.voxel.transform.position + direction * offset;

                GameObject newVoxel = Instantiate(sel.voxel, newPos, Quaternion.identity);
                newVoxel.transform.parent = sel.voxel.transform.parent; // Set the parent
                newVoxel.name = "Voxel"; // Make sure name is "Voxel"

                // Reset materials for the new voxel (clear any previous highlights)
                MeshRenderer renderer = newVoxel.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    Material[] mats = renderer.materials;
                    for (int i = 0; i < mats.Length; i++)
                    {
                        mats[i].color = Color.white;
                    }
                    renderer.materials = mats;
                }

                // Add new selection with the same face index
                VoxelSelection newSelection = new VoxelSelection(newVoxel, sel.normal, sel.faceIndex);
                SelectedVoxels.Add(newSelection);

                // Apply highlight to the new selection
                ApplyHighlight(newSelection);
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
                Vector3 direction = -sel.normal.normalized; // Opposite direction
                float offset = sel.voxel.transform.localScale.x * 1.1f;

                if (Physics.Raycast(sel.voxel.transform.position, direction, out RaycastHit hit, offset))
                {
                    GameObject hitVoxel = hit.collider.gameObject;

                    // Determine face index from triangle index
                    int faceIndex = hit.triangleIndex / 2;
                    faceIndex = Mathf.Clamp(faceIndex, 0, 5); // Ensure valid range

                    VoxelSelection newSelection = new VoxelSelection(hitVoxel, hit.normal, faceIndex);
                    SelectedVoxels.Add(newSelection);

                    // Apply highlight to the new selection
                    ApplyHighlight(newSelection);
                }

                Destroy(sel.voxel);
            }
        }
    }
}

using UnityEngine;

public enum VoxelType
{
    Terrain, // This Voxel is part of the chamber terrain (walls, floors, etc)
    Object // This Voxel is a placeable object (buttons, switches, etc)
}

public class Voxel
{
    /// <summary>
    /// The type of this Voxel.
    /// </summary>
    public VoxelType Type;

    /// <summary>
    /// The GameObject that represents this Voxel.
    /// </summary>
    public GameObject GameObject;

    // Constructor for Type
    public Voxel(VoxelType type) => Type = type;

    // Constructor for object types
    public Voxel(VoxelType type, GameObject gameObject) => (Type, GameObject) = (type, gameObject);
}


/// <summary>
/// Utility class for voxel data.
/// </summary>
public static class VoxelUtility
{
    // Backing field
    private static Mesh _voxelMesh;

    /// <summary>
    /// The mesh used for all voxels. 
    /// This can be set to override the default voxel mesh.
    /// </summary>
    public static Mesh VoxelMesh { 
        get 
        {
            if (_voxelMesh == null)
                GenerateVoxelMesh();

            return _voxelMesh;
        }

        set => _voxelMesh = value; 
    }

    // Generate the default Voxel Mesh
    // This gets called by the VoxelMesh getter
    private static void GenerateVoxelMesh()
    {
        // Set default voxel mesh (6 sided cube)
        _voxelMesh = new Mesh();
        _voxelMesh.name = "VoxelCube";

        // 24 vertices, 4 per face (6 faces, so 6 * 4 = 24)
        Vector3[] vertices = new Vector3[24]
        {
            // Front face
            new Vector3(-0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f),

            // Back face
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),

            // Left face
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),

            // Right face
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),

            // Top face
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),

            // Bottom face
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f),
        };

        // Six submeshes
        int[] frontFaceIndices = new int[] { 0, 1, 2, 0, 2, 3 };
        int[] backFaceIndices = new int[] { 4, 5, 6, 4, 6, 7 };
        int[] leftFaceIndices = new int[] { 8, 9, 10, 8, 10, 11 };
        int[] rightFaceIndices = new int[] { 12, 13, 14, 12, 14, 15 };
        int[] topFaceIndices = new int[] { 16, 17, 18, 16, 18, 19 };
        int[] bottomFaceIndices = new int[] { 20, 21, 22, 20, 22, 23 };

        // Combining all the indices (we'll add them to the submeshes later)
        int[][] allFaceIndices = new int[][]
        {
            frontFaceIndices, backFaceIndices, leftFaceIndices, rightFaceIndices, topFaceIndices, bottomFaceIndices
        };

        // Normals (same for all faces but different direction for each face)
        Vector3[] normals = new Vector3[24]
        {
            Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,     // Front
            Vector3.back, Vector3.back, Vector3.back, Vector3.back,                 // Back
            Vector3.left, Vector3.left, Vector3.left, Vector3.left,                 // Left
            Vector3.right, Vector3.right, Vector3.right, Vector3.right,             // Right
            Vector3.up, Vector3.up, Vector3.up, Vector3.up,                         // Top
            Vector3.down, Vector3.down, Vector3.down, Vector3.down                  // Bottom
        };

        // UVs (same for all faces, simple planar mapping)
        Vector2[] uvs = new Vector2[24]
        {
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),

            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),

            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),

            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),

            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1),

            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(1, 1), new Vector2(0, 1)
        };

        // Set the vertices, normals, and uvs for the whole mesh
        _voxelMesh.vertices = vertices;
        _voxelMesh.normals = normals;
        _voxelMesh.uv = uvs;

        // Create 6 submeshes (one per face)
        _voxelMesh.subMeshCount = 6;

        // Assign indices for each submesh
        _voxelMesh.SetTriangles(frontFaceIndices, 0);
        _voxelMesh.SetTriangles(backFaceIndices, 1);
        _voxelMesh.SetTriangles(leftFaceIndices, 2);
        _voxelMesh.SetTriangles(rightFaceIndices, 3);
        _voxelMesh.SetTriangles(topFaceIndices, 4);
        _voxelMesh.SetTriangles(bottomFaceIndices, 5);

        // Recalculate bounds for the mesh
        _voxelMesh.RecalculateBounds();
    }
}

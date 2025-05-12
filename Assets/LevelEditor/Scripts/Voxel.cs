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

        int[] indices = new int[36]
        {
            0, 2, 1, 0, 3, 2,         // Front
            4, 6, 5, 4, 7, 6,         // Back
            8,10, 9, 8,11,10,         // Left
            12,14,13,12,15,14,        // Right
            16,18,17,16,19,18,        // Top
            20,22,21,20,23,22         // Bottom
        };

        Vector3[] normals = new Vector3[24]
        {       
            Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,     // Front
            Vector3.back, Vector3.back, Vector3.back, Vector3.back,                 // Back
            Vector3.left, Vector3.left, Vector3.left, Vector3.left,                 // Left
            Vector3.right, Vector3.right, Vector3.right, Vector3.right,             // Right
            Vector3.up, Vector3.up, Vector3.up, Vector3.up,                         // Top
            Vector3.down, Vector3.down, Vector3.down, Vector3.down                  // Bottom
        };

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

        _voxelMesh.vertices = vertices;
        _voxelMesh.triangles = indices;
        _voxelMesh.normals = normals;
        _voxelMesh.uv = uvs;

        _voxelMesh.RecalculateBounds();
    }
}

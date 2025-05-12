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
            // Front face
            0, 1, 2, 0, 2, 3,
    
            // Back face
            4, 5, 6, 4, 6, 7,
    
            // Left face
            8, 9,10, 8,10,11,
    
            // Right face
           12,13,14,12,14,15,
    
            // Top face
           16,17,18,16,18,19,
    
            // Bottom face
           20,21,22,20,22,23
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

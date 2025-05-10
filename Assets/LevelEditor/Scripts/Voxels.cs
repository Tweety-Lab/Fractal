using UnityEngine;

/// <summary>
/// Represents a 3D Level Editor Voxel.
/// </summary>
public struct Voxel
{
    public Material FloorMaterial { get; set; }
    public Material WallMaterial { get; set; }
}

/// <summary>
/// Utility for Voxel Manipulation.
/// </summary>
public static class VoxelUtility
{
    public static Mesh CreateVoxelCube()
    {
        Mesh mesh = new Mesh();
        mesh.name = "VoxelMesh";

        Vector3[] vertices = {
            // Front face (0)
            new(-0.5f, -0.5f,  0.5f), new( 0.5f, -0.5f,  0.5f),
            new( 0.5f,  0.5f,  0.5f), new(-0.5f,  0.5f,  0.5f),
            // Back face (1)
            new( 0.5f, -0.5f, -0.5f), new(-0.5f, -0.5f, -0.5f),
            new(-0.5f,  0.5f, -0.5f), new( 0.5f,  0.5f, -0.5f),
            // Left face (2)
            new(-0.5f, -0.5f, -0.5f), new(-0.5f, -0.5f,  0.5f),
            new(-0.5f,  0.5f,  0.5f), new(-0.5f,  0.5f, -0.5f),
            // Right face (3)
            new( 0.5f, -0.5f,  0.5f), new( 0.5f, -0.5f, -0.5f),
            new( 0.5f,  0.5f, -0.5f), new( 0.5f,  0.5f,  0.5f),
            // Top face (4)
            new(-0.5f,  0.5f,  0.5f), new( 0.5f,  0.5f,  0.5f),
            new( 0.5f,  0.5f, -0.5f), new(-0.5f,  0.5f, -0.5f),
            // Bottom face (5)
            new(-0.5f, -0.5f, -0.5f), new( 0.5f, -0.5f, -0.5f),
            new( 0.5f, -0.5f,  0.5f), new(-0.5f, -0.5f,  0.5f)
        };
        mesh.vertices = vertices;

        // One quad (2 triangles) per face, using submeshes
        int[][] tris = new int[6][];
        for (int i = 0; i < 6; i++)
        {
            int vi = i * 4;
            tris[i] = new int[] { vi, vi + 1, vi + 2, vi, vi + 2, vi + 3 };
        }

        mesh.subMeshCount = 6;
        for (int i = 0; i < 6; i++)
        {
            mesh.SetTriangles(tris[i], i);
        }

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < 6; i++)
        {
            int vi = i * 4;
            uvs[vi + 0] = new Vector2(0, 0);
            uvs[vi + 1] = new Vector2(1, 0);
            uvs[vi + 2] = new Vector2(1, 1);
            uvs[vi + 3] = new Vector2(0, 1);
        }

        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
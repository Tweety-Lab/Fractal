using UnityEngine;

public enum VoxelType
{
    Terrain, // This Voxel is part of the chamber terrain (walls, floors, etc)
    Object // This Voxel is a placeable object (buttons, switches, etc)
}

public struct Voxel
{
    public VoxelType Type;
}

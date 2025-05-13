using UnityEngine;

public enum MountType
{
    /// <summary>No special mount logic.</summary>
    None,

    /// <summary>Attached to floor, will move up and down with attached voxel.</summary>
    Floor,

    /// <summary>Attached to wall, will move with attached voxel.</summary>
    Wall,

    /// <summary>Attached to ceiling, will move with attached voxel.</summary>
    Ceiling
}

public class EditorPackage : MonoBehaviour
{
    [Header("Behaviour")]
    [Tooltip("The mount type of the package.")]
    public MountType MountType;

    // Voxel this inhabits
    public Vector3Int Position { get; set; }

    // Voxel this is mounted on
    public Vector3Int MountPoint { get; set; }

    public void UpdatePosition(VoxelWorld voxelWorld)
    {
        Debug.Log("Updating position");

        switch(MountType)
        {
            case MountType.Floor:
                Debug.Log("Updating Floor");

                // Mount on floor
                MountPoint = Position - Vector3Int.up;

                // Check if mountpoint is still there
                if (!voxelWorld.TryGetVoxel(MountPoint, out Voxel voxel))
                {
                    Debug.Log("VoxelWorld: Mount point no longer exists, falling down.");
                    Vector3Int newPosition = Position - Vector3Int.up; // Fall down
                    voxelWorld.MoveVoxel(Position, newPosition); // Move to new position
                    Position = newPosition;
                    Debug.Log("VoxelWorld: Position updated to " + Position);
                }

                break;
        }
    }
}

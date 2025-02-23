using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "ObjDatabase", menuName = "Scriptable Objects/ObjDatabase")]
public class ObjDatabase : ScriptableObject
{
    public List<ObjectData> objectsData;
}
[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string name {  get; private set; }
    [field: SerializeField]
    public bool isWall { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public Material[] Materials { get; private set; }
}

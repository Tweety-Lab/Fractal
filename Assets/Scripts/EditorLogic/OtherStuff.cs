using UnityEngine;
using System;

[CreateAssetMenu(fileName = "OtherStuff", menuName = "Scriptable Objects/OtherStuff")]
public class OtherStuff : ScriptableObject
{
    public Stuff stuffData;
}
[Serializable]
public class Stuff
{
    [field: SerializeField]
    public Material[] MirrorMats { get; private set; }
    [field: SerializeField]
    public GameObject ReflProbe { get; private set; }
}

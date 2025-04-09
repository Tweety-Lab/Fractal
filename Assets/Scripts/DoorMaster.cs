using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMaster : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Door open sound ID. Seek ID in soundscape manager node.")]
    public int openSoundID;
    [Tooltip("Door close sound ID. Seek ID in soundscape manager node.")]
    public int closeSoundID;
    [SerializeField]
    public bool OpenAtStart;
    public sound_node soundPlayer;
    private void Start()
    {
        if (OpenAtStart)
        {
            Open_Door();
        }
    }
    public void Open_Door()
    {
        soundPlayer.PlayAudio(openSoundID);
        base.GetComponent<Animation>().Play("OpenDoor");
    }
    public void Close_Door()
    {
        soundPlayer.PlayAudio(closeSoundID);
        base.GetComponent<Animation>().Play("CloseDoor");
    }
}

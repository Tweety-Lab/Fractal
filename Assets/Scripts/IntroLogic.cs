using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class IntroLogic : MonoBehaviour
{
    public VideoPlayer clip;
    public sound_node Sound;
    public int SndID;
    public UnityEvent On_FinishIntro;
    async private void Start()
    {
        await Task.Delay(1000);
        clip.Play();
        await Task.Delay(11000);
        On_FinishIntro.Invoke();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            On_FinishIntro.Invoke();
        }
    }
}

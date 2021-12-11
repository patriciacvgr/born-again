using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClick;

    public void PlayButtonClick()
    {
        SoundManager.Instance.PlaySound(buttonClick);
    }

    public static void StopMusic()
    {
        SoundManager.Instance._musicSource.Stop();
    }

    public static void StartMusic()
    {
        SoundManager.Instance._musicSource.Play();
    }
}

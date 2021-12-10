using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private bool isMasterVolume, isMusic, isEffect;

    void Start()
    {
        SoundManager.Instance.ChangeMasterVolume(_slider.value);
        SoundManager.Instance.ChangeMusicVolume(_slider.value);
        SoundManager.Instance.ChangeEffectsVolume(_slider.value);

        if (isMasterVolume) _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));

        if (isMusic) _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));

        if (isEffect) _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectsVolume(val));
    }
}

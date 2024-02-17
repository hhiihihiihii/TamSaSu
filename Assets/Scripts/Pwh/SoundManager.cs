using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DisgendPattern;
using UnityEngine.UI;

public class SoundManager : SingleTon<SoundManager>
{
    [Header("SoundClip")]
    [SerializeField] AudioSource _bgmClip;
    [SerializeField] AudioSource _effectClip;

    [Header("UI")]
    [SerializeField] Slider _bgm;
    [SerializeField] Slider _effect;

    void Start()
    {

    }

    void Update()
    {
        _bgmClip.volume = _bgm.value;
        _effectClip.volume = _effect.value;
    }
}

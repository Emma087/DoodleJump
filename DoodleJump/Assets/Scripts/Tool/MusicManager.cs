using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景音乐的播放
/// </summary>
public class MusicManager : MonoSingleton<MusicManager>
{
    [HideInInspector]
    public AudioSource _audioSource;
    public AudioClip bgMusic;
    public bool isGameover = false;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    public void PlayMusic()
    {
        _audioSource.clip = bgMusic;
        _audioSource.Play();
    }

    private void Update()
    {
        if (isGameover)
        {
            if (_audioSource.pitch > 0.4f)
                _audioSource.pitch -= Time.deltaTime * 0.5f;
            else
            {
                isGameover = false;
            }
        }
    }
}
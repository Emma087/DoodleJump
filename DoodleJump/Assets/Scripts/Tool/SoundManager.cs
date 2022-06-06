using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioClip[] AudioClips;
    //0 UI点击，1 吃到金币，2 花钱，3 吃到道具飞起来，4 出错了或者买不起东西，5 踩每一个格子，6 tile坏掉的声音，7选择皮肤
    
    [HideInInspector]
    public AudioSource _audioSource;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放音效，根据编号，编号在引擎赋值
    /// </summary>
    /// <param name="id"></param>
    public void PlayAudioClips(int id)
    {
        _audioSource.PlayOneShot(AudioClips[id]);
    }
}
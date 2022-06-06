using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 专门用来控制 UI皮肤跳跃的
/// </summary>
public class UISkinJump : MonoBehaviour
{
    private float offsetY = 160; //Y轴偏移
    private float time = 0.4f; //动画的时间长

    private void Start()
    {
        Init();
    }

    void Init()
    {
        iTween.MoveBy(gameObject, iTween.Hash(
            "y", offsetY,
            "easeType", iTween.EaseType.easeInOutQuad,
            "loopType", iTween.LoopType.pingPong,
            "time", time
        ));
        GetComponent<iTween>().enabled = false;
    }

    /// <summary>
    /// 播放跳跃动画
    /// </summary>
    public void Resume()
    {
        GetComponent<iTween>().enabled = true;
    }

    /// <summary>
    /// 暂停动画
    /// </summary>
    public void Pause()
    {
        GetComponent<iTween>().enabled = false;
    }
}
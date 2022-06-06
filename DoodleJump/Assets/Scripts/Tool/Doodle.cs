using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doodle : MonoBehaviour
{
    private float offsetY = 200; //Y轴偏移
    private float time = 0.8f; //动画的时间长

    void Start()
    {
        iTween.MoveBy(gameObject, iTween.Hash(
            "y", offsetY,
            "easeType", iTween.EaseType.easeInOutQuad,
            "loopType", iTween.LoopType.pingPong,
            "time", time
        ));
    }
}
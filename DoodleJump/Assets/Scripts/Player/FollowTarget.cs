using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    Vector3 velocity = Vector3.zero;
    private float dampTime = 0.5f;

    Vector3 targetViewPos = Vector3.zero; //目标的位置，
    Vector3 delta = Vector3.zero; //目标和视口坐标中心的差值，以此差值来驱使相机的平滑移动（主角超过了视口坐标中心多少）
    Vector3 destination = Vector3.zero; //目的地

    void Awake()
    {
        ResetSize();
    }

    static float designWidth = 720;
    static float designHeight = 1280;
    static float designPixelOfUnit = 80;

    public static void ResetSize()
    {
        float zoomRate = Screen.width / designWidth; //当前宽度 / 设计宽度 = 当前宽度和设计宽的的 比例
        float currentPixelOfUnit = designPixelOfUnit * zoomRate; //当前的 Unit单位像素值
        float currentScreenWhRate = (float) Screen.width / (float) Screen.height; //当前的尺寸宽高比

        float screenWidthUnit = (float) Screen.width / currentPixelOfUnit; //宽度有几个 Unit单元格
        float screenHeightUnit = screenWidthUnit / currentScreenWhRate; //以宽度的单元格 算出高度的单元格数量

        float cameraSize = screenHeightUnit / 2; //高度的单元格 / 2 = 相机的显示比例
        Camera.main.orthographicSize = cameraSize;
    }

    private void Update()
    {
        if (target) //如果发现了目标
        {
            targetViewPos = Camera.main.WorldToViewportPoint(target.position); //这句不能省，如果直接使用 target的 position，会产生错误
            delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, targetViewPos.z));
            destination = transform.position + delta;
            destination.x = 0; //移动不往左右两边使劲
            if (destination.y > transform.position.y)
            {
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
        }
    }
}
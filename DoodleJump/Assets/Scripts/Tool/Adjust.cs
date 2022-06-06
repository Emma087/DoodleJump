using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此脚本是动态修改图片，以适应屏幕
public class Adjust : MonoBehaviour
{
    void Start()
    {
        Resize();
    }

    /// <summary>
    /// 动态修改地面的图片，以适应贴合屏幕
    /// </summary>
    void Resize()
    {
        float width = GetComponent<SpriteRenderer>().bounds.size.x; //这个是地面草坪图片的宽度
        float targetWidth = Camera.main.orthographicSize * 2 / Screen.height * Screen.width;
        //Vector3 scale = transform.localScale;
        //scale.x = targetWidth / width;
        //transform.localScale = scale;
        transform.localScale = new Vector3(targetWidth / width, transform.localScale.y, transform.localScale.z);
    }

}
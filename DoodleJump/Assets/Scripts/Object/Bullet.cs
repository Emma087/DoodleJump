using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    int bulletSpeed = 15;

    private void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; //每次跳之前，要将之前的速度清零，才能保证每次的子弹发射高度一样
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1,1), bulletSpeed), ForceMode2D.Impulse);
    }

    private void Update()
    {
        //关于子弹回收的功能
        if (GameManager.Instance.floor.transform.position.y > transform.position.y + 1)
            GameManager.Instance.AddInActiveObjectToPool(gameObject, ObjectType.Bullet);
    }
}
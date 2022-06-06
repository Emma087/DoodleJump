using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.AddInActiveObjectToPool(gameObject, ObjectType.Coin);
            GameManager.Instance.Coin++;
            SoundManager.Instance.PlayAudioClips(2);
        }
    }

    private void Update()
    {
        //关于金币回收的功能
        if (GameManager.Instance.floor.transform.position.y > transform.position.y + 1)
            GameManager.Instance.AddInActiveObjectToPool(gameObject, ObjectType.Coin);
    }
}
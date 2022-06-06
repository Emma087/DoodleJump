using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed; //敌人速度
    private float distance; //敌人移动的范围距离
    private Vector3 startPosotion; //敌人移动的起始位置
    private int direction; //敌人运动方向

    public float[] Speed; //3个敌人的移动速度的数组
    public float[] Distance;
    public Sprite[] Sprites;
    private SpriteRenderer _spriteRenderer; //指 敌人当前的图片是哪一张
    private int enemyType; //0是小红 1是黄蜻蜓 2是小蓝

    private void OnEnable() //当 tile的 go.SetActive(true) 显示为真时候，以下代码就会执行
    {
        Init();
    }

    void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        enemyType = Int32.Parse(gameObject.name); //生成的 敌人有自己的编号名字，转成 int然后更改图片}
        _spriteRenderer.enabled = true;
        _spriteRenderer.sprite = Sprites[enemyType];
        speed = Speed[enemyType];
        startPosotion = transform.position;
        distance = Distance[enemyType];
    }

    /// <summary>
    /// 当有碰撞体碰到了敌人以后
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        //如果碰到敌人的是，主角，主角就死翘翘
        if (other.tag == "Player")
        {
            GameObject player = other.gameObject;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Collider2D>().enabled = false;
            player.GetComponent<Rigidbody2D>().gravityScale = 3;
            Invoke("GameOverInvoke",1.5f);
        }

        //如果碰到敌人的是子弹，（并且子弹是朝上的时候，才生效，朝下打到敌人不算）敌人就一命呜呼
        if (other.tag == "Bullet" && other.GetComponent<Rigidbody2D>().velocity.y >= 0)
        {
            GameManager.Instance.AddInActiveObjectToPool(gameObject, ObjectType.Enemy);
            GameManager.Instance.AddInActiveObjectToPool(other.gameObject, ObjectType.Bullet);
            GameManager.Instance.Score += 10;
        }
    }

    private void GameOverInvoke()
    {
        GameManager.Instance.EndGame();
    }

    private void Update()
    {
        #region 关于 敌人横着走的状态

        if (direction == 0) //向左走
        {
            transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
            if (startPosotion.x - transform.position.x > distance)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y,
                    transform.localScale.z);
                direction = 1;
            }
        }
        else //direction == 1，向右走
        {
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
            if (startPosotion.x - transform.position.x < -distance) //往右走，相当于向量是负的，所以要和-distance比
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y,
                    transform.localScale.z);
                direction = 0;
            }
        }

        #endregion

        #region 关于敌人的回收功能

        if (GameManager.Instance.floor.transform.position.y > transform.position.y + 1)
        {
            GameManager.Instance.AddInActiveObjectToPool(gameObject, ObjectType.Enemy);
        }

        #endregion
    }
}
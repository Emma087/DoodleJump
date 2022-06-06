using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileType;
    // 0代表普通的绿色瓦片，1是坏的瓦片，2是黄色瓦片，3是红色瓦片，4是蓝色瓦片，5也是蓝色瓦片（功能不一样）

    public Sprite[] sprites; //这是6个图片的图片资源数组，在引擎中赋值
    private SpriteRenderer _spriteRenderer; //指 tile当前的图片是哪一张

    private float speed; //横着和竖着的条，移动的速度
    private float distance; //横条竖条最多移动的范围
    private Vector3 startPosotion; //横条竖条的起始位置
    private int direction; //横竖条的方向，0代表左和上，1代表右和下

    private void OnEnable() //当 tile的 go.SetActive(true) 显示为真时候，以下代码就会执行
    {
        Init();
    }

    void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        tileType = Int32.Parse(gameObject.name); //生成的 tile有自己的编号名字，转成 int然后更改图片
        switch (tileType)
        {
            case 0:
                _spriteRenderer.sprite = sprites[0];
                break;
            case 1:
                _spriteRenderer.sprite = sprites[1];
                break;
            case 2:
                _spriteRenderer.sprite = sprites[2];
                break;
            case 3:
                _spriteRenderer.sprite = sprites[3];
                break;
            case 4: //横条
                _spriteRenderer.sprite = sprites[4];
                speed = GameManager.Instance.Advance.movingHorizontally.speedMove;
                distance = GameManager.Instance.Advance.movingHorizontally.distanceMove;
                startPosotion = transform.position;
                break;
            case 5: //竖条
                _spriteRenderer.sprite = sprites[5];
                speed = GameManager.Instance.Advance.movingVertically.speedMove;
                distance = GameManager.Instance.Advance.movingVertically.distanceMove;
                startPosotion = transform.position;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 是否有东西碰到了 tile，如果碰到 tile的是玩家
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        //other.GetComponent<Rigidbody2D>().velocity.y < 0  玩家的速度，朝下，才触发，向上不触发
        if (other.tag == "Player" && other.GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            switch (tileType)
            {
                case 0: //普通的格子，就让主角再跳上去
                    other.GetComponent<Player>().Jump();
                    break;
                case 1:
                    GetComponent<Rigidbody2D>().gravityScale = 1.5f;
                    SoundManager.Instance.PlayAudioClips(6);
                    break;
                case 2:
                    other.GetComponent<Player>().Jump();
                    GetComponent<Rigidbody2D>().gravityScale = 1.5f;
                    break;
                case 3:
                    other.GetComponent<Player>().Jump(1.5f);
                    break;
                case 4:
                    other.GetComponent<Player>().Jump();
                    break;
                case 5:
                    other.GetComponent<Player>().Jump();
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        #region 关于 横着走和竖着走的 tile走的状态
        switch (tileType)
        {
            case 4:
                if (direction == 0)
                {
                    transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
                    if (startPosotion.x - transform.position.x > distance)
                        direction = 1;
                }
                else
                {
                    transform.Translate(new Vector2(speed * Time.deltaTime, 0));
                    if (startPosotion.x - transform.position.x < -distance)  //往右走，相当于向量是负的，所以要和-distance比
                        direction = 0;
                }

                break;
            case 5:
                if (direction == 0)
                {
                    transform.Translate(new Vector2(0, -speed * Time.deltaTime));
                    if (startPosotion.y - transform.position.y > distance)
                        direction = 1;
                }
                else
                {
                    transform.Translate(new Vector2(0, speed * Time.deltaTime));
                    if (startPosotion.y - transform.position.y < -distance)
                        direction = 0;
                }

                break;
            default:
                break;
        }

        #endregion

        #region 关于 tile的回收功能

        if (GameManager.Instance.floor.transform.position.y > transform.position.y + 1)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0f;
            GameManager.Instance.AddInActiveObjectToPool(gameObject, ObjectType.Tile);
        }

        #endregion
    }
}
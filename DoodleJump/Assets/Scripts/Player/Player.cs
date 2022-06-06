using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private float jumpForceNumber = 12f;
    private float playerMoveSpeed = 5f;

    private float rightBorder; //右侧边界
    private float leftBorder; //左侧边界

    private SpriteRenderer _spriteRenderer;
    public Sprite fire; //主角开火时候的样子
    public Sprite normal; //主角最初始的模样

    [HideInInspector] public bool isFlying; //道具是否在飞，这里是重复的声明，为了限制飞的时候不能开火

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).x;
        rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).x;
        GameManager.Instance.SelectSkin(); //主角换装
    }

    /// <summary>
    /// 检测是否触碰到了地面，如果触碰到了，那么就调用跳跃
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Platform")
        {
            Jump();
        }

        if (other.tag == "Floor")
        {
            GameManager.Instance.EndGame();
        }
    }

    /// <summary>
    /// 主角的跳跃函数，参数为正常模式，还是吃到火箭的模式，x 代表力的大小
    // </summary>
    public void Jump(float x = 1f)
    {
        if (GameManager.Instance.GameState == GameState.GameOver)
            return;

        GetComponent<Rigidbody2D>().velocity = Vector2.zero; //每次跳之前，要将之前的速度清零，才能保证每次的跳跃高度一样
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForceNumber * x), ForceMode2D.Impulse);
        SoundManager.Instance.PlayAudioClips(5);
    }


    Vector3 playerFaceDirection = Vector3.zero; //玩家的脸朝向
    private float parameterNumber;
    private Vector3 targetPosition;

    private float intervalTime = 0.3f; //显示开火时间
    private float currentTime;

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Running)
        {
            #region 主角的运动控制代码，主角的运动不能超过边界代码

            playerFaceDirection = Vector3.zero;
            parameterNumber = 0;
            if (Input.GetKey(KeyCode.LeftArrow) ||Input.GetKey(KeyCode.A))
            {
                parameterNumber = -1;
                transform.localScale = new Vector2(parameterNumber, transform.localScale.y);
            }

            if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D))
            {
                parameterNumber = 1;
                transform.localScale = new Vector2(parameterNumber, transform.localScale.y);
            }

            playerFaceDirection.x = parameterNumber;
            // targetPosition.y = transform.position.y;
            // targetPosition.z = 0;
            targetPosition = Vector3.MoveTowards(transform.position, transform.position + playerFaceDirection,
                playerMoveSpeed * Time.deltaTime);

            //以下的代码设置，不让主角 x轴超过边界
            if (targetPosition.x < leftBorder)
            {
                targetPosition.x = rightBorder;
            }

            if (targetPosition.x > rightBorder)
            {
                targetPosition.x = leftBorder;
            }

            transform.position = targetPosition;

            #endregion

            #region 主角发射子弹相关
            
            currentTime += Time.deltaTime;
            //!EventSystem.current.IsPointerOverGameObject() 是 UI防止穿透，点击 UI时候，发射不了子弹
            if (Input.GetMouseButtonDown(0) && !isFlying && !EventSystem.current.IsPointerOverGameObject())
            {
                if (currentTime < intervalTime)
                {
                    return;
                }

                GameManager.Instance.GetInactiveObject(ObjectType.Bullet);
                _spriteRenderer.sprite = fire;
                currentTime = 0;
            }

            if (Input.GetMouseButtonUp(0))
            {
                GameManager.Instance.SelectSkin(); //主角换装
            }
            #endregion
        }
    }
}
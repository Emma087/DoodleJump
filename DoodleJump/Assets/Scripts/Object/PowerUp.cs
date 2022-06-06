using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int itemType; // 0代表帽子，1代表火箭

    [HideInInspector] public SpriteRenderer _spriteRenderer; //指 tile当前的图片是哪一张，这是引擎手拖的

    public Sprite[] sprites; //这是 2个图片的图片资源数组，在引擎中赋值
    public float[] powerTime; //每个道具存在的时间
    public GameObject[] used; //道具生效期间，显示哪一个图片
    
    private bool isFlying; //是否正在播放某道具的飞，是否是飞的状态
    private void OnEnable() //当 道具 item的 go.SetActive(true) 显示为真时候，以下代码就会执行
    {
        Init();
    }

    void Init()
    {
        _spriteRenderer.enabled = true;
        itemType = Int32.Parse(gameObject.name); //生成的 tile有自己的编号名字，转成 int然后更改图片}
        _spriteRenderer.sprite = sprites[itemType];
        isFlying = false;
    }

    private GameObject player;

    //是否有碰到物体，如果是玩家碰到这个道具，就触发以下代码
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            player.GetComponent<Rigidbody2D>().velocity =
                player.GetComponent<Rigidbody2D>().velocity / 3; //玩家的速度归零，因为需要道具让玩家匀速往上走，但是归零效果不好，更改为 1/3倍
            
            player.GetComponent<Rigidbody2D>().isKinematic = true; //开启玩家动力学，为了不影响向上走的速度，也会忽视其他的 tile
            GetComponent<Collider2D>().enabled = false; //道具生效期间，关闭道具的 collider
            player.GetComponent<Collider2D>().enabled = false;
            _spriteRenderer.enabled = false; //吃到的那个道具，隐藏
            used[itemType].SetActive(true); //显示对应的道具实施的效果图片
            isFlying = true;
            player.GetComponent<Player>().isFlying = true;
            
            SoundManager.Instance.PlayAudioClips(3);
            StartCoroutine("StopFly");
        }
    }

    /// <summary>
    /// 用来关闭道具，帽子或者火箭道具效果的协程
    /// </summary>
    /// <returns></returns>
    IEnumerator StopFly()
    {
        yield return new WaitForSeconds(powerTime[itemType]);
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Collider2D>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
        used[itemType].SetActive(false); //对应的道具实施的效果图片，关闭掉
        isFlying = false;
        player.GetComponent<Player>().isFlying = false;
        
        SoundManager.Instance._audioSource.Stop();  //停止音效的那个音，这么写可以的，不会影响其他的
    }

    private void Update()
    {
        #region 关于 item的回收功能

        if (isFlying)
            player.transform.Translate(new Vector2(0, 12 * Time.deltaTime));
        //之所以这里用 else if 不是 if，是因为防止在飞行过程中，被回收，那会造成飞行的中断
        else if (GameManager.Instance.floor.transform.position.y > transform.position.y + 1)
            GameManager.Instance.AddInActiveObjectToPool(gameObject, ObjectType.Item);

        #endregion
    }
}
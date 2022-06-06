using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    //游戏的初始状态
    public GameState GameState = GameState.Running;
    public GameObject playerGameObject; //游戏的主角

    private void Start()
    {
        GenerateItemPool();
        GenerateCoinPool();
        GenerateEnemyPool();
        GenerateBulletPool();
        Debug.Log("对象池初始化完成");

        GetAllWeight(); //totalsum 权重总数的赋值
        GenerateTilePool(); //瓦片的对象池生成
        // 生成 tile物体
        for (int i = 0; i < initialSize; i++)
        {
            GenerateTile(); //先一次性生成一批 tile
        }

        GenerateItem(); //生成物品
        GenerateEnemy(); //生成敌人
        GenerateCoin(); //生成金币
        
        GUIManager.Instance.GUIPlayerSelectSkin();
        // SelectSkin();  //主角换装
    }

    // tile的对象池
    public Queue<GameObject> tilePool = new Queue<GameObject>();
    public Queue<GameObject> coinPool = new Queue<GameObject>();
    public Queue<GameObject> bulletPool = new Queue<GameObject>();
    public Queue<GameObject> enemyPool = new Queue<GameObject>();
    public Queue<GameObject> itemPool = new Queue<GameObject>();

    public GameObject floor; //判定是否回收 tile的边界，也是判定是否玩家掉下去的边界
    public Transform objectParent; //除了 tile以外所有物体的父物体

    

    #region tile生成相关代码

    /// <summary>
    /// 获取所有 tile的权值，然后加到一个总数里，也就是 totalsum中
    /// </summary>
    void GetAllWeight()
    {
        float sum = 0;
        sum += Advance.normalTile.weight;
        sum += Advance.brokenTile.weight;
        sum += Advance.oneTimeOnly.weight;
        sum += Advance.springTile.weight;
        sum += Advance.movingVertically.weight;
        sum += Advance.movingVertically.weight;
        totalsum = sum;
    }

    public GameObject tilePrefabs; // tile的预制体
    public GameSetting Advance; //前进的路

    private int initialSize = 60; // tile池的数量

    /// <summary>
    /// 生成 tile 池子，根据对象池的数量
    /// </summary>
    void GenerateTilePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject go = Instantiate(tilePrefabs, transform); //父物体可以酌情改一下，现在是 gamemanager
            go.SetActive(false);
            go.name = i.ToString();
            tilePool.Enqueue(go);
        }
    }

    float currentPositionY = 2; //当前生成物体的 Y轴高度
    private float totalsum; // 几种 tile出现的总共权值

    /// <summary>
    /// 生成随机砖块
    /// </summary>
    void GenerateTile()
    {
        GameObject go = GetInactiveObject(ObjectType.Tile); //根据物体的枚举类型，取出 Tile
        float random = Random.Range(9, totalsum);
        int tileRandomNumber = SetTileByRandomNumber(random); //根据随机数，获得是哪一种 tile，得到其编号

        Vector2 randomPosition = new Vector2(Random.Range(-4f, 4f), currentPositionY);
        switch (tileRandomNumber)
        {
            case 0:
                go.transform.position = randomPosition;
                currentPositionY += Random.Range(Advance.normalTile.minBornHeight, Advance.normalTile.maxBornHeight);
                go.name = "0";
                go.SetActive(true);
                break;
            case 1:
                go.transform.position = randomPosition;
                currentPositionY += Random.Range(Advance.brokenTile.minBornHeight, Advance.brokenTile.maxBornHeight);
                go.name = "1";
                go.SetActive(true);
                break;
            case 2:
                go.transform.position = randomPosition;
                currentPositionY += Random.Range(Advance.oneTimeOnly.minBornHeight, Advance.oneTimeOnly.maxBornHeight);
                go.name = "2";
                go.SetActive(true);
                break;
            case 3:
                go.transform.position = randomPosition;
                currentPositionY += Random.Range(Advance.springTile.minBornHeight, Advance.springTile.maxBornHeight);
                go.name = "3";
                go.SetActive(true);
                break;
            case 4:
                go.transform.position = randomPosition;
                currentPositionY += Random.Range(Advance.movingHorizontally.minBornHeight,
                    Advance.movingHorizontally.maxBornHeight);
                go.name = "4";
                go.SetActive(true);
                break;
            case 5:
                go.transform.position = randomPosition;
                currentPositionY += Random.Range(Advance.movingVertically.minBornHeight,
                    Advance.movingVertically.maxBornHeight);
                go.name = "5";
                go.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 根据随机数字，计算要生成哪一个 tile，返回的是对应 tile的编号
    /// </summary>
    int SetTileByRandomNumber(float number)
    {
        if (number <= Advance.normalTile.weight)
            return 0;
        else if (number <= Advance.normalTile.weight + Advance.brokenTile.weight)
            return 1;
        else if (number <= Advance.normalTile.weight + Advance.brokenTile.weight
                                                     + Advance.oneTimeOnly.weight)
            return 2;
        else if (number <= Advance.normalTile.weight + Advance.brokenTile.weight
                                                     + Advance.oneTimeOnly.weight
                                                     + Advance.springTile.weight)
            return 3;
        else if (number <= Advance.normalTile.weight + Advance.brokenTile.weight
                                                     + Advance.oneTimeOnly.weight
                                                     + Advance.springTile.weight
                                                     + Advance.movingHorizontally.weight)
            return 4;
        else if (number <= Advance.normalTile.weight + Advance.brokenTile.weight
                                                     + Advance.oneTimeOnly.weight
                                                     + Advance.springTile.weight
                                                     + Advance.movingHorizontally.weight
                                                     + Advance.movingVertically.weight)
            return 5;

        //以上情况都不符合的话，默认返回一个 -1
        return -1;
    }

    #endregion

    /// <summary>
    /// 当一个 tile销毁以后，调用这个方法，来生成新的 tile
    /// </summary>
    void CreateTile()
    {
        if (GameState != GameState.GameOver)
        {
            GenerateTile(); //生成新的 Tile
            GenerateItem(); //生成新的 奖励物品
            GenerateEnemy(); //生成新的怪物
            GenerateCoin(); //生成金币
            IncreseDifficult(15);
            Score += 5;
            //分数
        }
    }

    #region item生成相关

    public GameObject itemPrefabs; // item的预制体
    public Transform itemParent;
    private int itemNumber = 10;

    /// <summary>
    /// 随机生成 奖励物品
    /// </summary>
    void GenerateItem()
    {
        if (Random.Range(0f, 1f) < Advance.itemProbability)
        {
            GameObject randomGO = null;
            while (true) //死循环，作用是限制，奖励物品不能太靠下，而且奖励物品不能在会移动的 Tile上面
            {
                randomGO = transform.GetChild(Random.Range(0, initialSize)).gameObject;
                if (randomGO.GetComponent<Tile>().tileType < 4 &&
                    randomGO.transform.position.y > Camera.main.transform.position.y)
                    break;
            }

            GameObject go = GetInactiveObject(ObjectType.Item);
            go.name = Random.Range(0, 2).ToString();
            go.SetActive(true);
            go.transform.position = randomGO.transform.position + new Vector3(0, 0.6f, 0);
        }
    }

    /// <summary>
    /// 生成 item 池子
    /// </summary>
    void GenerateItemPool()
    {
        for (int i = 0; i < itemNumber; i++)
        {
            GameObject go = Instantiate(itemPrefabs, itemParent);
            go.SetActive(false);
            go.name = i.ToString();
            itemPool.Enqueue(go);
        }
    }

    #endregion

    #region enemy敌人生成相关

    public GameObject enemyPrefabs; // enemy的预制体
    public Transform enemyParent;

    private int enemyNumber = 10;

    /// <summary>
    /// 随机生成 敌人
    /// </summary>
    void GenerateEnemy()
    {
        if (Random.Range(0f, 1f) < Advance.enemyProbability)
        {
            GameObject go = GetInactiveObject(ObjectType.Enemy);
            go.name = Random.Range(0, 3).ToString();
            go.SetActive(true);
            go.transform.position = new Vector2(Random.Range(-3.5f, 3.5f), currentPositionY);
        }
    }

    /// <summary>
    /// 生成 enemy 池子，根据对象池的数量
    /// </summary>
    void GenerateEnemyPool()
    {
        for (int i = 0; i < enemyNumber; i++)
        {
            GameObject go = Instantiate(enemyPrefabs, enemyParent); //父物体可以酌情改一下，现在是 gamemanager
            go.SetActive(false);
            go.name = i.ToString();
            enemyPool.Enqueue(go);
        }
    }

    #endregion

    #region bullet子弹生成相关

    public GameObject bulletPrefabs; // bullet的预制体
    private int bulletNumber = 30;
    public Transform bulletShootPosition; // bullet发射的位置
    public Transform bulletParent;

    /// <summary>
    /// 生成 bullet 池子，根据对象池的数量
    /// </summary>
    void GenerateBulletPool()
    {
        for (int i = 0; i < bulletNumber; i++)
        {
            GameObject go = Instantiate(bulletPrefabs, bulletParent); //父物体可以酌情改一下，现在是 gamemanager
            go.SetActive(false);
            go.name = i.ToString();
            bulletPool.Enqueue(go);
        }
    }

    #endregion

    #region 各个对象从对象池中取出（出队）和各个对象回收进对象池中（入队）

    /// <summary>
    /// 回收物体，将物体隐藏，并且收回到 各个物体对象池中
    /// </summary>
    public void AddInActiveObjectToPool(GameObject go, ObjectType type)
    {
        go.SetActive(false);
        switch (type)
        {
            case ObjectType.Tile:
                tilePool.Enqueue(go); //回收到 tilePoll对象池中（入队 tile对象池）
                CreateTile(); //然后生成新的
                break;
            case ObjectType.Item:
                itemPool.Enqueue(go);
                break;
            case ObjectType.Coin:
                coinPool.Enqueue(go);
                break;
            case ObjectType.Enemy:
                enemyPool.Enqueue(go);
                break;
            case ObjectType.Bullet:
                bulletPool.Enqueue(go);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 从对象池中，取物体
    /// </summary>
    public GameObject GetInactiveObject(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Tile:
                return tilePool.Dequeue();
            case ObjectType.Item:
                return itemPool.Dequeue();
            case ObjectType.Coin:
                return coinPool.Dequeue();
            case ObjectType.Enemy:
                return enemyPool.Dequeue();
            case ObjectType.Bullet:
                GameObject go = bulletPool.Dequeue();
                go.transform.position = bulletShootPosition.position;
                go.SetActive(true);
                return go;
            default:
                return null;
        }
    }

    #endregion

    #region coin 金币相关，金币的增加

    public Transform coinParent;
    private int coin;

    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            UIGameWindow.UpdateCoinForGameWindow(value);
        }
    }

    public GameObject coinPrefabs; // coin的预制体

    /// <summary>
    /// 随机生成 金币
    /// </summary>
    void GenerateCoin()
    {
        if (Random.Range(0f, 1f) < Advance.coinProbability)
        {
            GameObject go = GetInactiveObject(ObjectType.Coin);
            go.name = "coin";
            go.SetActive(true);
            go.transform.position = new Vector2(Random.Range(-4.5f, 4.5f), currentPositionY);
        }
    }

    /// <summary>
    /// 生成 coin 池子，根据对象池的数量
    /// </summary>
    void GenerateCoinPool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject go = Instantiate(coinPrefabs, coinParent); //父物体可以酌情改一下，现在是 gamemanager
            go.SetActive(false);
            go.name = i.ToString();
            coinPool.Enqueue(go);
        }
    }

    #endregion

    #region 游戏的开始，游戏结束函数，游戏面板的分数和金币的实时更新（只是 UI实时更新，金币的增减，看哪里调用了 Score）

    public UI_Game UIGameWindow; //更新分数的代码，写到了游戏分数声明的变量属性中，更新金币代码，写到了游戏金币声明的变量属性中

    /// <summary>
    /// 游戏开始调用
    /// </summary>
    public void PlayGame()
    {
        playerGameObject.SetActive(true);
        GameState = GameState.Running;
        //读取皮肤信息
    }

    //游戏结束，两种情况，1，主角碰到了敌人就游戏结束，2，主角掉下去了游戏结束
    public void EndGame()
    {
        Debug.Log("游戏结束了");
        if (GameState != GameState.GameOver)
        {
            GameState = GameState.GameOver;
            //相机抖动效果
            iTween.ShakePosition(Camera.main.gameObject, new Vector3(0.3f, 0.3f, 0), 0.8f);
            UIGameWindow.ShowFlash();
            if (PlayerPrefs.HasKey("BestScore"))
            {
                //当前分，超过了历史最高分，就将新的分数刷新进历史最高分中
                if (score > PlayerPrefs.GetInt("BestScore"))
                    PlayerPrefs.SetInt("BestScore", score);
            }
            else
            {
                PlayerPrefs.SetInt("BestScore", score);
            }

            PlayerPrefs.SetInt("Coin", coin + PlayerPrefs.GetInt("Coin", 100));
            StartCoroutine(EndGameHelp());
            MusicManager.Instance.isGameover = true;
        }
    }

    IEnumerator EndGameHelp()
    { 
        yield return new WaitForSeconds(1f);
        GUIManager.Instance.OpenPanel(3, true); //打开结束面板
        MusicManager.Instance._audioSource.Stop();
        SoundManager.Instance._audioSource.Stop();
    }

    #endregion

    #region 游戏的难度曲线变化，游戏分数声明

    private float lastTime; //上一次时间

    /// <summary>
    /// 游戏的难度曲线变化
    /// </summary>
    void IncreseDifficult(float time)
    {
        if (Time.time - lastTime > time)
        {
            lastTime = Time.time;
            Advance.enemyProbability += 0.01f;
            //Debug.Log("游戏难度增加0.01，当前的难度为" + Advance.enemyProbability);
        }
    }

    private int score; //游戏的分数

    public int Score
    {
        get => score;
        set
        {
            score = value;
            UIGameWindow.UpdateScoreForGameWindow(value);
        }
    }

    #endregion


    /// <summary>
    /// 主角换装，换成商店购买的那个图片
    /// </summary>
    public void SelectSkin()
    {
        playerGameObject.GetComponent<SpriteRenderer>().sprite =
            SkinManager.Instance.Skins[PlayerPrefs.GetInt("SelectSkin", defaultValue: 0)].SpriteCharacter;
    }
}


//游戏状态机
public enum GameState
{
    Paused,
    Running,
    GameOver,
}

//场景中物体的枚举类型分类
public enum ObjectType
{
    Tile, //主角踩的瓦片
    Item, //火箭一类的道具
    Coin, //金币
    Enemy, //敌人
    Bullet, //子弹
}
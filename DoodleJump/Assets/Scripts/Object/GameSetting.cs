using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 游戏的设置，用来设置，Tile的相关属性
/// </summary>
[Serializable]
public class GameSetting
{
    #region 关于 Tile所有自定义类型的声明
    
    [Serializable]
    public class NormalTile //普通的绿色瓦片（不会掉落，也永远都不消失），Tile的编号为 0
    {
        //随机出生的最小距离高度
        public float minBornHeight;

        //随机出生的最大距离高度
        public float maxBornHeight;

        //生成的概率大小（权值）
        public float weight;
    }

    [Serializable]
    public class BrokenTile //坏的瓦片（一次都踩不了，一踩就掉了），Tile的编号为 1
    {
        public float minBornHeight;
        public float maxBornHeight;
        public float weight;
    }

    [Serializable]
    public class OneTimeOnly //黄色瓦片（踩一次就消失），Tile的编号为 2
    {
        public float minBornHeight;
        public float maxBornHeight;
        public float weight;
    }

    [Serializable]
    public class SpringTile //红色瓦片（踩一次能跳的很高），Tile的编号为 3
    {
        public float minBornHeight;
        public float maxBornHeight;
        public float weight;
    }

    [Serializable]
    public class MovingHorizontally //蓝色横着移动的瓦片，Tile的编号为 4
    {
        public float minBornHeight;
        public float maxBornHeight;
        public float weight;

        //移动的范围距离
        public float distanceMove;

        //移动的速度
        public float speedMove;
    }

    [Serializable]
    public class MovingVertically //蓝色竖着移动的瓦片，Tile的编号为 5
    {
        public float minBornHeight;
        public float maxBornHeight;
        public float weight;

        //移动的范围距离
        public float distanceMove;

        //移动的速度
        public float speedMove;
    }
    #endregion

    #region Tile所有类型对象的创建
    
    public NormalTile normalTile;
    public BrokenTile brokenTile;
    public OneTimeOnly oneTimeOnly;
    public SpringTile springTile;
    public MovingHorizontally movingHorizontally;
    public MovingVertically movingVertically;
    #endregion

    #region 奖励物品，金币，敌人出现的概率
    
    public float itemProbability; //奖励物品的掉落几率（引擎赋值 0.03，代表出现概率 3%）
    public float coinProbability; //金币的掉落几率（引擎赋值 0.2，代表出现概率 20%）
    public float enemyProbability; //敌人出现的概率（引擎赋值 0.03，代表出现概率 3%）
    #endregion
}
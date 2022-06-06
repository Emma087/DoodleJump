using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoSingleton<SkinManager>
{
    
    /// <summary>
    /// 皮肤的数据，自定义类
    /// </summary>
    [Serializable]
    public class Skin
    {
        public int price;  //价格
        public bool isDefauleLock = false;  //是否解锁，默认是否，不解锁
        public Sprite SpriteCharacter;  //当前是哪一张皮肤
    }

    public List<Skin> Skins = new List<Skin>();  //包含 6个皮肤的 List列表，里面包含每一个皮肤的细节信息
}

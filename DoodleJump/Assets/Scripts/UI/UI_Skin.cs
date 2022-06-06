using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skin : MonoBehaviour
{
    public Text txt_coin; //引擎内金币数量的文本

    private void OnEnable()
    {
        Init();
        RefreshSkinItemList();
    }
    
    public void TestAddCoin()
    {
        //一下是测试代码，用完删除，按一下 Q就加 1000金币
        PlayerPrefs.SetInt("Coin", 1000);
        Init();
        SoundManager.Instance.PlayAudioClips(0);
    }

    void Init()
    {
        txt_coin.text = PlayerPrefs.GetInt("Coin", defaultValue: 100).ToString();
    }

    public List<GameObject> SkinItems; //皮肤界面的 6个皮肤，用一个 List装

    /// <summary>
    /// 更新皮肤界面，每一个皮肤的详细信息
    /// </summary>
    void RefreshSkinItemList()
    {
        for (int i = 0; i < SkinManager.Instance.Skins.Count; i++)
        {
            //更改主角图片
            SkinItems[i].transform.Find("character").GetComponent<Image>().sprite =
                SkinManager.Instance.Skins[i].SpriteCharacter;
            SkinItems[i].transform.Find("character").GetComponent<Image>().SetNativeSize();

            //判断有没有解锁，0为锁着的，1为解锁，这里的条件是，如果没有解锁
            if (PlayerPrefs.GetInt("IsSkin" + i + "UnLocked", defaultValue: 0) == 0
                && !SkinManager.Instance.Skins[i].isDefauleLock)
            {
                //将对应皮肤的价格的物体显示
                SkinItems[i].transform.Find("Image").gameObject.SetActive(true);
                //显示对应皮肤的价格
                SkinItems[i].transform.Find("Image/Text").GetComponent<Text>().text =
                    SkinManager.Instance.Skins[i].price.ToString();
            }
            else //如果已经解锁了
            {
                //将对应皮肤的价格的物体，隐藏
                SkinItems[i].transform.Find("Image").gameObject.SetActive(false);
            }

            SkinItems[i].name = i.ToString(); //这里修改名字为数字，是为了 OnBtnClick中使用
        }
    }

    /// <summary>
    /// 每一个 item按钮点击以后的代码逻辑
    /// </summary>
    /// <param name="id"> 当前选择的物体 </param>
    public void OnBtnClick(GameObject id)
    {
        Debug.Log("当前选择的皮肤编号为"+id.name);
        foreach (var skinItem in SkinItems)
        {
            if (skinItem == id) //循环 SkinItemsList中所有的 item，如果等于当前选中的这个 item的 id，就播放跳跃的动画
                skinItem.GetComponent<UISkinJump>().Resume();
            else
                skinItem.GetComponent<UISkinJump>().Pause(); //如果不是，就暂停动画
        }

        SkinManager.Skin skin = SkinManager.Instance.Skins[Int32.Parse(id.name)];

        //如果皮肤状态是解锁的，而且存储的数值也是解锁的，这个皮肤有了
        if (skin.isDefauleLock ||
            PlayerPrefs.GetInt("IsSkin" + Int32.Parse(id.name) + "UnLocked", defaultValue: 0) == 1)
        {
            //更换成选中的那个皮肤
            SelectSkin(Int32.Parse(id.name));
            SoundManager.Instance.PlayAudioClips(7);
        }
        else //如果还没有拥有，就买下来
        {
            //如果皮肤的价格，小于兜里有的钱，那么就买下皮肤，并且扣钱
            if (skin.price <= PlayerPrefs.GetInt("Coin", 100))
            {
                //扣钱，而且将扣完的钱，存进数据中
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin", 100) - skin.price);
                //把买完的那个皮肤，存进数据中
                PlayerPrefs.SetInt("IsSkin" + Int32.Parse(id.name) + "UnLocked", 1);
                SelectSkin(Int32.Parse(id.name));
                Init();
                Debug.Log("买成功了");
                SoundManager.Instance.PlayAudioClips(2);
            }
            else //如果买不起
            {
                //播放音乐
                Debug.Log("买不起");
                SoundManager.Instance.PlayAudioClips(4);
            }
        }
    }

    public UI_Mian UIMain;

    /// <summary>
    /// 根据 id号选择某一个皮肤
    /// </summary>
    /// <param name="id"></param>
    void SelectSkin(int id)
    {
        PlayerPrefs.SetInt("SelectSkin", id);
        UIMain.RefreshSkin();
        RefreshSkinItemList();
    }

    public void OnBtnReturn()
    {
        GUIManager.Instance.Back();
        SoundManager.Instance.PlayAudioClips(0);
    }
}
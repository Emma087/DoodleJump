using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Mian : MonoBehaviour
{
    public Image player;


    private void OnEnable()
    {
        RefreshSkin();
    }

    public void TestDeleteAllPlayerPrefabs()
    {
        //一下是测试代码，用完删除，按一下 Q就加 1000金币

        PlayerPrefs.DeleteAll();
        SoundManager.Instance.PlayAudioClips(0);
        SceneManager.LoadScene(0);
    }

    public void RefreshSkin()
    {
        //主角换装
        player.GetComponent<Image>().sprite =
            SkinManager.Instance.Skins[PlayerPrefs.GetInt("SelectSkin", defaultValue: 0)].SpriteCharacter;
        //  Debug.Log("主角皮肤编号为",SkinManager.Instance.Skins[PlayerPrefs.GetInt("SelectSkin", defaultValue: 0)].SpriteCharacter);
        player.GetComponent<Image>().SetNativeSize();
    }

    public void OnBtnPlay()
    {
        GUIManager.Instance.OpenPanel(1, true);
        GameManager.Instance.PlayGame();
        SoundManager.Instance.PlayAudioClips(0);
    }

    public void OnBtnShop()
    {
        GUIManager.Instance.OpenPanel(4);
        SoundManager.Instance.PlayAudioClips(0);
    }
}
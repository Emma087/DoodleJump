using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    public void OnBtnMenu()
    {
        // GUIManager.Instance.OpenPanel(0,true);
        SceneManager.LoadScene(0);
        SoundManager.Instance.PlayAudioClips(0);
    }

    public Text txt_Best; //最好的分数
    public Text txt_Score; //当前的分数
    public Text txt_Coin; //本次获得的金币

    private void OnEnable()
    {
        txt_Best.text = PlayerPrefs.GetInt("BestScore", defaultValue: 0).ToString();
        txt_Score.text = GameManager.Instance.Score.ToString();
        txt_Coin.text = GameManager.Instance.Coin.ToString();
    }
}
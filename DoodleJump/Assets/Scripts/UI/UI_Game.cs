using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Game : MonoBehaviour
{
    public Text txt_coin;
    public Text txt_score;

    private void OnEnable()
    {
        txt_coin.text = "0";
        txt_score.text = 0.ToString();
    }

    public void UpdateCoinForGameWindow(int coinNumber) //游戏面板的实时更新金币
    {
        txt_coin.text = coinNumber.ToString();
    }

    public void UpdateScoreForGameWindow(int scoreNumber) //游戏面板的实时更新分数
    {
        txt_score.text = scoreNumber.ToString();
    }

    public void OnBtnPause()
    {
        GameManager.Instance.GameState = GameState.Paused;
        Time.timeScale = 0;
        GUIManager.Instance.OpenPanel(2);
        SoundManager.Instance.PlayAudioClips(0);
    }

    /// <summary>
    /// 虚晃一下白色的背景
    /// </summary>
    public void ShowFlash()
    {
        StartCoroutine(ShowFlashI());
    }

    public GameObject white;

    IEnumerator ShowFlashI()
    {
        white.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        white.SetActive(false);
    }
}
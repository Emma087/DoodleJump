using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pause : MonoBehaviour
{
    public Text txt_coin;
    public Text txt_score;

    public void OnBtnResume()
    {
        //关闭暂停界面时候，将游戏的状态更改为 running，游戏的速度更改为 1
        GameManager.Instance.GameState = GameState.Running;
        Time.timeScale = 1;
        GUIManager.Instance.Back();
        SoundManager.Instance.PlayAudioClips(0);
    }

    private void OnEnable()
    {
        txt_coin.text = GameManager.Instance.Coin.ToString();
        txt_score.text = GameManager.Instance.Score.ToString();
    }
}
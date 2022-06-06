using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoSingleton<GUIManager>
{
    public List<GameObject> panels; //所有的面板
    // 0是Main，1是Game，2是游戏暂停，3是游戏结束，4是皮肤商店
    public Stack<GameObject> PanelStack = new Stack<GameObject>();
    public int currentNumber; //当前显示哪一个界面

    public GameObject currentPanelObject
    {
        get { return PanelStack.Peek(); }
    }
    // private void OnEnable()
    // {
    //     Init();  //调用这个会报错，因为这个启动快于 GameManager皮肤编号赋值，所以报空
    // }

    public void GUIPlayerSelectSkin()
    {
        Debug.Log("Main场景调用了");
        OpenPanel(currentNumber); //默认是 0.也就是主界面
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    /// <param name="id">第几个面板</param>
    /// <param name="isHidePrivious">是否隐藏前一个面板，默认不隐藏</param>
    public void OpenPanel(int id, bool isHidePrvious = false)
    {
        // A 和 B没有前后影响的关系
        if (isHidePrvious)
        {
            if (PanelStack.Peek() != null) //返回顶部元素，但不删除它
            {
                currentPanelObject.SetActive(false);
                PanelStack.Pop(); //移除并且返回顶部元素
            }

            PanelStack.Push(panels[id]);
            currentPanelObject.SetActive(true);
        }
        else //A和B 有前后遮挡影响关系
        {
            PanelStack.Push(panels[id]);
            currentPanelObject.SetActive(true);
        }
    }

    /// <summary>
    /// 返回上一次的面板
    /// </summary>
    public void Back()
    {
        if (PanelStack.Peek() != null)
        {
            currentPanelObject.SetActive(false);
            PanelStack.Pop();
        }

        currentPanelObject.SetActive(true);
    }
}
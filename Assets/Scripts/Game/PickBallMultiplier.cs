using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickBallMultiplier : MonoBehaviour
{
    public GameObject panel;
    public List<Button> buttons;
    public delegate void OnSelectBallMultiplier(int index);
    public OnSelectBallMultiplier onSelectBallMultiplier;

    void Start()
    {
        foreach(var button in buttons)
        {
            int index = button.transform.GetSiblingIndex();
            button.onClick.AddListener(()=>OnButtonPickBall(index));
        }
    }

    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    void OnButtonPickBall(int index)
    {
        SoundManager.PlayClick();
        if (onSelectBallMultiplier != null)
        {
            onSelectBallMultiplier(index);
        }
        else
        {
            Debug.Log("OnButtonPickBall failed delefate not defined!");
        }

        Hide();
    }
}

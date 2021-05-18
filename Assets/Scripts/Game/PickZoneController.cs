using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickZoneController : MonoBehaviour
{
    public GameObject panel;
    public List<Button> buttons;
    public delegate void OnSelectPickZone(int index);
    public OnSelectPickZone onSelectPickZone;

    void Start()
    {
        foreach (var button in buttons)
        {
            int index = button.transform.GetSiblingIndex();
            button.onClick.AddListener(() => OnButtonPickZone(index));
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

    void OnButtonPickZone(int index)
    {
        SoundManager.PlayClick();
        if (onSelectPickZone != null)
        {
            onSelectPickZone(index);
        }
        else
        {
            Debug.Log("OnButtonPickBall failed delefate not defined!");
        }

        Hide();
    }
}

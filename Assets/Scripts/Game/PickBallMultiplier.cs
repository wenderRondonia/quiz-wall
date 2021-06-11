using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickBallMultiplier : MonoBehaviour
{
    public GameObject panel;
    public List<Button> buttons;
    [Header("Runtime")]
    public int indexAnswered = -1;

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
        indexAnswered = -1;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    void OnButtonPickBall(int index)
    {
        SoundManager.PlayClick();
        indexAnswered = index;
       

    }

    public IEnumerator WaitingAnswer()
    {
        yield return new WaitUntil(() => indexAnswered != -1);

        Hide();

    }
}

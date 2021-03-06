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

    [Header("Runtime")]
    public int zoneSelected;

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
        zoneSelected = -1;
        QuestionScreen.instance.InitTimer();
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    void OnButtonPickZone(int index)
    {
        SoundManager.PlayClick();
        zoneSelected = index;
        QuestionScreen.instance.StopTimer();
        Hide();
    }

    public IEnumerator WaitingAnswer()
    {
        yield return new WaitUntil(() => zoneSelected != -1);


    }

    public void SelectRandomAnswer()
    {
        zoneSelected = Random.Range(0, 6);
        Debug.Log("PickZoneController.SelectRandomAnswer");

    }

}

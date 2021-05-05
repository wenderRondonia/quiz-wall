using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSreen : MonoBehaviour
{
    [SerializeField] Button back;
    void Start()
    {
        back.onClick.AddListener(OnClickBack);
    }

    void OnClickBack(){

        SoundManager.PlayClick();
        back.Focus();
        LoadScreenManager.instance.LoadSceneScreen("Menu");
        SoundManager.PlayMusicMenu();
        SoundManager.StopMusicGame();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    [SerializeField] Button back;

    void Awake()
    {
        back.onClick.AddListener(OnClickBack);

    }

    void Start()
    {
        SoundManager.PlayMusicGame();

        StartCoroutine(MainGameLoop());
       
    }


    void OnClickBack(){

        SoundManager.PlayClick();
        back.Focus();
        
        LoadScreenManager.instance.LoadSceneScreen("Map");
        SoundManager.StopMusicGame();

    }


    IEnumerator MainGameLoop()
    {
        yield return RoundScreen.ShowingNewRound();

        yield return BallController.instance.DoAnimatingBalls();
    }

}

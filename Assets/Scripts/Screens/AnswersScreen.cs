using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswersScreen : Singleton<AnswersScreen>
{

    public GameObject panel;
    public List<Button> buttons;
    public List<bool> correctAnswers;
    public Sprite spriteCorrect;
    public Sprite spriteIncorrect;

    void Start()
    {

    }

   
    public void Show()
    {
        FadePanel.FadeIn(panel);
        StartCoroutine(ShowingCorrectAnswers());
    }

    IEnumerator ShowingCorrectAnswers()
    {

        yield return new WaitForSeconds(2);

        for (int i=0; i < 4; i++ )
        {
            buttons[i].image.sprite = correctAnswers[i] ? spriteCorrect : spriteIncorrect ;
            yield return new WaitForSeconds(0.5f);

        }
    }

}

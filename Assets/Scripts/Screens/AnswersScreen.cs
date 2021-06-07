using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswersScreen : Singleton<AnswersScreen>
{

    public GameObject panel;
    public List<Button> buttons;
    public Sprite spriteCorrect;
    public Sprite spriteIncorrect;
    
    [Header("Runtime")]
    
    public List<bool> correctAnswers;


    void Start()
    {

    }

    public void Hide()
    {
        FadePanel.FadeOut(panel);

    }
    public void Show()
    {
        FadePanel.FadeIn(panel);
    }

    public IEnumerator ShowingCorrectAnswers()
    {

        Show();

        yield return new WaitForSeconds(2);

        for (int i=0; i < 4; i++ )
        {
            bool isCorrect = correctAnswers[i];
            
            buttons[i].image.sprite = isCorrect ? spriteCorrect : spriteIncorrect ;
            

            yield return new WaitForSeconds(0.5f);

        }

        yield return new WaitForSeconds(3);

        Hide();

    }

}

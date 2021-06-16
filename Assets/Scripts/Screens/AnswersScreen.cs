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
    
    public Button continueButton;

    [Header("Runtime")]
    
    public List<bool> correctAnswers;

    bool clickedContinue;

    void Start()
    {
        continueButton.onClick.AddListener(OnClickContinue);
    }

    void OnClickContinue()
    {
        SoundManager.PlayClick();
        clickedContinue = true;
    }

    public void Hide()
    {
        FadePanel.FadeOut(panel);

    }
    public void Show()
    {
        clickedContinue = false;
        FillAnswers();
        FadePanel.FadeIn(panel);
    }

    void FillAnswers()
    {
        foreach (var button in buttons)
        {
            int index = buttons.IndexOf(button);
            int lastIndex = QuestionScreen.instance.questionHistory.Count - 4 + index;
            QuestionData lastQuestionData = QuestionScreen.instance.questionHistory[lastIndex];
            button.GetComponentInChildByName<Text>("TextQuestion").text = lastQuestionData.question;
            button.GetComponentInChildByName<Text>("TextAnswer").text = lastQuestionData.GetRightAnswerText;
        }
    }

    public IEnumerator ShowingCorrectAnswers()
    {

        Show();

        yield return new WaitForSeconds(1);

        for (int i=0; i < 4; i++ )
        {
            bool isCorrect = correctAnswers[i];
            
            buttons[i].image.sprite = isCorrect ? spriteCorrect : spriteIncorrect ;

            

            yield return new WaitForSeconds(0.5f);

        }

        yield return new WaitUntil(()=>clickedContinue);

        Hide();

    }

}

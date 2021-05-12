using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : BaseScreen<QuestionScreen>
{
    public Text textTitle;
    public Text textQuestion;
    public List<Button> buttonAnswers;
    QuestionData currentQuestionData;
    int currentQuestionNumber;
    
    void Start()
    {
        foreach (var buttonAnswer in buttonAnswers)
        {
            int index = buttonAnswer.transform.GetSiblingIndex();
            buttonAnswer.onClick.AddListener(()=>OnButtonAnswer(index));
        }
    }

    void OnButtonAnswer(int index)
    {
        SoundManager.PlayClick();

    }

    public void SetupQuestion(int questionNumber, int questionCount,QuestionData questionData)
    {
        currentQuestionData = questionData;
        currentQuestionNumber = questionNumber;

        textTitle.text = "QUESTION " + questionNumber + "/" + questionCount;
        textQuestion.text = questionData.question;
        for(int i=0; i < questionData.answers.Length;i++)
        {
            var textAnswer = buttonAnswers[i].GetComponentInChildren<Text>();
            textAnswer.text = questionData.answers[i];
        }
    }


}

[System.Serializable]
public class QuestionData
{
    public string question;
    public string[] answers;
    public int rightAnswer;

}

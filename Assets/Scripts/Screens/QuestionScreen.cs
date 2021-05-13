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
    int indexAnswered=-1;
    
    void Start()
    {
        foreach (var buttonAnswer in buttonAnswers)
        {
            int index = buttonAnswer.transform.GetSiblingIndex();
            buttonAnswer.onClick.AddListener(()=>OnButtonAnswer(index));
        }
    }

    public static IEnumerator WaitingQuestionAnswer()
    {
        yield return new WaitUntil(() => instance.indexAnswered != -1);
    }

    public override void Show()
    {
        base.Show();
        indexAnswered = -1;
        currentQuestionData = null;
    }

    void OnButtonAnswer(int index)
    {
        SoundManager.PlayClick();
        indexAnswered = index;
        
        this.ExecuteIn(0.5f, () => { Hide(); });

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
            buttonAnswers[i].gameObject.SetActive(true);
        }

        //deactive unused buttons
        for (int i = questionData.answers.Length; i < buttonAnswers.Count;i++)
        {
            buttonAnswers[i].gameObject.SetActive(false);
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

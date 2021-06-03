using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : BaseScreen<QuestionScreen>
{
    public Text textTitle;
    public Text textQuestion;
    public Sprite spriteDefault;
    public Sprite spriteCorrect;
    public Sprite spriteIncorrect;
    public List<Button> buttonAnswers;

    public AudioSource SoundRightAnswer;
    public AudioSource SoundWrongAnswer;

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

        //ResetQuestion();
    }

    public void ResetQuestion()
    {
        indexAnswered = -1;
        currentQuestionData = null;

        foreach (var buttonAnswer in buttonAnswers)
        {
            buttonAnswer.interactable = true;
        }

    }

    void OnButtonAnswer(int index)
    {
        if (indexAnswered != -1)
        {
            Debug.Log("OnButtonAnswer already indexAnswered=" + indexAnswered);
            return;
        }

        SoundManager.PlayClick();
        indexAnswered = index;
        buttonAnswers[index].image.color = Color.gray;
        buttonAnswers[index].interactable = false;
        this.ExecuteIn(0.8f, () => { Hide(); });

    }


    public void ShowAnswersPreview()
    {
        if (currentQuestionData == null)
        {
            Debug.Log("ShowAnswersPreview failed: no question data");
            return;
        }
        for (int i = 0; i < currentQuestionData.AnswersShuffled.Count; i++)
        {
            var buttonAnswer = buttonAnswers[i];
            buttonAnswer.image.color = new Color(1,1,1,0.5f);
            buttonAnswer.image.sprite = spriteDefault;
            buttonAnswer.interactable = false;
            buttonAnswer.gameObject.SetActive(true);
            buttonAnswer.GetComponentInChildren<Text>(true).gameObject.SetActive(false);
        }

        //deactive unused buttons
        for (int i = currentQuestionData.AnswersShuffled.Count; i < buttonAnswers.Count; i++)
        {
            var buttonAnswer = buttonAnswers[i];

            buttonAnswer.gameObject.SetActive(false);
        }
    }


    public void ShowAnswers()
    {
        for (int i = 0; i < currentQuestionData.AnswersShuffled.Count; i++)
        {
            var buttonAnswer = buttonAnswers[i];
            buttonAnswer.image.color = Color.white;
            buttonAnswer.interactable = true;
            buttonAnswer.GetComponentInChildren<Text>(true).gameObject.SetActive(true);
        }
    }


    public void ShowCorrectAnswer()
    {

        int correctAnswerIndex = currentQuestionData.rightAnswer;

        for (int i = 0; i < currentQuestionData.AnswersShuffled.Count; i++)
        {
            var buttonAnswer = buttonAnswers[i];

            buttonAnswer.image.color = Color.white;

            int index = buttonAnswer.transform.GetSiblingIndex();

            if(index == indexAnswered || index == correctAnswerIndex)
            {
                buttonAnswer.image.sprite = index == correctAnswerIndex ? spriteCorrect : spriteIncorrect;
            }
          
        }

        if (correctAnswerIndex == indexAnswered)
        {
            SoundRightAnswer.Play();
        }
        else
        {
            SoundWrongAnswer.Play();
        }

    }


    public void HideAnswers()
    {
        foreach (var buttonAnswer in buttonAnswers)
        {
            buttonAnswer.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Fill UI with question data
    /// </summary>
    /// <param name="questionNumber"></param>
    /// <param name="questionCount"></param>
    /// <param name="questionData"></param>
    public void SetupQuestion(int questionNumber,int questionCount, QuestionData questionData)
    {
        currentQuestionData = questionData;
        currentQuestionNumber = questionNumber;

        textTitle.text = "QUESTION " + questionNumber + "/" + questionCount;
        textQuestion.text = questionData.question;
        for(int i=0; i < questionData.AnswersShuffled.Count; i++)
        {
            var textAnswer = buttonAnswers[i].GetComponentInChildren<Text>(true);
            textAnswer.text = questionData.AnswersShuffled[i];
            
        }
    }


}




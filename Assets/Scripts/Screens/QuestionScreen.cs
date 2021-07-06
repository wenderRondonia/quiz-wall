using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : BaseScreen<QuestionScreen>
{
    public Text textTitle;
    public Text timerText;
    public Text textQuestion;
    public Sprite spriteDefault;
    public Sprite spriteCorrect;
    public Sprite spriteIncorrect;
    public List<Button> buttonAnswers;

    public PickBallMultiplier pickBallMultiplier;
    public PickZoneController pickZoneController;
    public AudioSource SoundRightAnswer;
    public AudioSource SoundWrongAnswer;
    public AudioSource SoundAnswered;

    public bool useTimer = true;

    [Header("Runtime")]
    public QuestionData currentQuestionData;
    public int currentQuestionNumber;
    public int indexAnswered = -1;

    public List<QuestionData> questionHistory = new List<QuestionData>();

    IEnumerator TimingCoroutine;
    public bool IsAnsweredRight()
    {
        if (currentQuestionData == null)
        {
            Debug.Log("IsAnsweredRight failed");
            return false;
        }

        return currentQuestionData.rightAnswer == indexAnswered;
    }



    void Start()
    {
        foreach (var buttonAnswer in buttonAnswers)
        {
            int index = buttonAnswer.transform.GetSiblingIndex();
            buttonAnswer.onClick.AddListener(() => OnButtonAnswer(index));
        }
    }

    public static bool IsAnswered()
    {
        return instance.indexAnswered != -1;
    }

    public static IEnumerator WaitingAnswer()
    {
        yield return new WaitUntil(() => instance.indexAnswered != -1);
    }

    public override void Show()
    {
        base.Show();

        //ResetQuestion();
        if (useTimer)
        {
            InitTimer();
        }
    }

    public void InitTimer()
    {
        StopTimer();
        timerText.gameObject.SetActive(true);

        TimingCoroutine = Timing();
        StartCoroutine(TimingCoroutine);

    }

    public void StopTimer()
    {
        timerText.gameObject.SetActive(false);

        if (TimingCoroutine != null)
        {
            StopCoroutine(TimingCoroutine);
            TimingCoroutine = null;
        }
    }

    IEnumerator Timing()
    {

        for (int i = 15; i >= 0; i--)
        {
            timerText.text = "00:" + i.ToString("00");
            yield return new WaitForSeconds(1);
        }

        if (pickZoneController.isActiveAndEnabled)
        {
            pickZoneController.SelectRandomAnswer();
            pickZoneController.Hide();
        }
        else if (pickBallMultiplier.isActiveAndEnabled)
        {
            pickBallMultiplier.SelectRandomAnswer();
            pickBallMultiplier.Hide();
        }
        else
        {
            SelectRandomAnswer();
            Hide();

        }

        StopTimer();

    }

    public void SelectRandomAnswer()
    {
        indexAnswered = Random.Range(0, currentQuestionData.answers.Length);

        Debug.Log("QuestionScreen.SelectRandomAnswer");

    }

    public void ResetQuestion()
    {
        indexAnswered = -1;
        currentQuestionData = null;


        foreach (var buttonAnswer in buttonAnswers)
        {
            buttonAnswer.interactable = true;
            buttonAnswer.image.color = Color.white;
            buttonAnswer.image.sprite = spriteDefault;
            buttonAnswer.gameObject.SetActive(true);
            buttonAnswer.GetComponentInChildren<Text>(true).gameObject.SetActive(true);
        }

    }

    public void HideQuestionText()
    {
        textQuestion.enabled = false;
    }

    public void ShowQuestionText()
    {
        textQuestion.enabled = true;
    }

    void OnButtonAnswer(int index)
    {
        if (indexAnswered != -1)
        {
            Debug.Log("OnButtonAnswer already indexAnswered=" + indexAnswered);
            return;
        }

        SoundAnswered.Play();

        indexAnswered = index;
        buttonAnswers[index].image.color = Color.gray;
        buttonAnswers[index].interactable = false;

        StopTimer();

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
            buttonAnswer.image.color = new Color(1, 1, 1, 0.5f);
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


    public void ShowAnswers(bool interactable = true)
    {
        for (int i = 0; i < currentQuestionData.AnswersShuffled.Count; i++)
        {
            var buttonAnswer = buttonAnswers[i];
            buttonAnswer.image.color = Color.white;
            buttonAnswer.interactable = interactable;
            buttonAnswer.GetComponentInChildren<Text>(true).gameObject.SetActive(true);
        }
    }


    public void ShowCorrectAnswer()
    {

        int correctAnswerIndex = currentQuestionData.rightAnswer;
        bool hasNotAnswered = indexAnswered == -1;

        for (int i = 0; i < currentQuestionData.AnswersShuffled.Count; i++)
        {
            var buttonAnswer = buttonAnswers[i];

            buttonAnswer.image.color = Color.white;

            int index = buttonAnswer.transform.GetSiblingIndex();

            if (hasNotAnswered)
            {
                buttonAnswer.image.sprite = spriteIncorrect;
            }
            else if (index == indexAnswered || index == correctAnswerIndex)
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
    public void SetupQuestion(int questionNumber, int questionCount, QuestionData questionData)
    {
        currentQuestionData = questionData;
        questionHistory.Add(questionData);
        currentQuestionNumber = questionNumber;

        Debug.Log("SetupQuestion=" + questionData + " AnswersShuffled=" + questionData.AnswersShuffled.ToStringValues());
        textTitle.text = "PERGUNTA" + " " + questionNumber + "/" + questionCount;
        textQuestion.text = questionData.question;
        for (int i = 0; i < questionData.AnswersShuffled.Count; i++)
        {
            var textAnswer = buttonAnswers[i].GetComponentInChildren<Text>(true);
            textAnswer.text = questionData.AnswersShuffled[i];

        }
    }

    public static IEnumerator DoingQuestionCheck()
    {
        QuestionScreen.instance.Show();
        QuestionScreen.instance.ShowCorrectAnswer();

        yield return new WaitForSeconds(2);

        QuestionScreen.instance.Hide();

        yield return new WaitForSeconds(1);

    }
}




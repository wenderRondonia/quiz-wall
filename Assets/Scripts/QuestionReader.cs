using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class QuestionData
{
    public string question;
    public string[] answers;
    public int rightAnswer;
    public int level;
    public string category;

    List<string> sortedAnswers;
    public List<string> AnswersShuffled { get {
        if (sortedAnswers==null)
        {
            string rightAnswerUsed = answers[rightAnswer];
            sortedAnswers = new List<string>(answers);
            sortedAnswers.Shuffle();
            rightAnswer = sortedAnswers.FindIndex(a=>a==rightAnswerUsed);
        }
        return sortedAnswers; 
    }}
}


public class QuestionReader
{
    public static List<QuestionData> questions = new List<QuestionData>();
    public static void ReadQuestions()
    {
        var questionsAsset = Resources.Load<TextAsset>("Questions");

        if (questionsAsset == null)
        {
            return;
        }

        var questionText = questionsAsset.text;

        string[,] questionDataMatrix = SplitCsvGrid(questionText);

        for (int i = 0; i < questionDataMatrix.GetLength(0); i++)
        {

            for (int j = 0; j < questionDataMatrix.GetLength(1); j++)
            {

                var storedValue = questionDataMatrix[i, j];
                if (storedValue != "" && storedValue != null)
                {
                    if (i >= 2 && j >= 2)
                    {
                        ProcessValue(storedValue, j - 2, i - 2);
                    }
                }

            }
        }

        //foreach (var question in questions)
        //    Debug.Log(questions.IndexOf(question) + " question=" + JsonUtility.ToJson(question, true));
        
    }

#if UNITY_EDITOR
    
    [UnityEditor.MenuItem("Tools/Test Questions")]
    public static void Test()
    {
        

    }

    static void ProcessValue(string storedValue, int row, int column)
    {
       
        
        if (row >= questions.Count)
        {
            questions.Add(new QuestionData() { answers = new string[4] { "", "", "", "" } });
        }

        switch (column)
        {
            case 0: questions[row].question = storedValue; break;
            case 1: questions[row].answers[0] = storedValue; break;
            case 2: questions[row].answers[1] = storedValue;  break;
            case 3: questions[row].answers[2] = storedValue; break;
            case 4:
                questions[row].answers[3] = storedValue;

                questions[row].rightAnswer = questions[row].AnswersShuffled.FindIndex(s=>s==storedValue); 
                break;
            case 5:
                int parsedValue = 0;
                bool succeed = int.TryParse(storedValue, out parsedValue);
                if (succeed) {
                    questions[row].level = parsedValue;
                }else{
                    Debug.Log("ProcessValue failed to parse column=" + column + " row=" + row + " storedValue=" + storedValue);
                }
                break;
            case 6: questions[row].category = storedValue; break;
            default:
                Debug.Log("ProcessValue failed column not identified="+column+" row="+row+" storedValue="+storedValue);
                break;

        }
        

    }

#endif



    // splits a CSV file into a 2D string array
    static public string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split("\n"[0]);

        // finds the max width of row
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[width + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];

                // This line was to replace "" with " in my output. 
                // Include or edit it as you wish.
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputGrid;
    }

    // splits a CSV row 
    static public string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }

}

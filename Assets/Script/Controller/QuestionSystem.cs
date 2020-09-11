using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class QuestionSystem
{
    Dictionary<int, List<int>> outed = new Dictionary<int, List<int>>();
    Dictionary<int, List<Question>> questions = new Dictionary<int, List<Question>>();
    public Question GetQuestion(int difficulty)
    {
        int rndNum;
        int n = questions[difficulty].Count;
        if (!outed.ContainsKey(difficulty)) outed.Add(difficulty, new List<int>());


        if (n == outed[difficulty].Count) return Question.Null;
        
        do { rndNum = new System.Random().Next(n); } while (outed[difficulty].Contains(rndNum));
        outed[difficulty].Add(n);
        return questions[difficulty][rndNum];
    }

    public void CSVRead()
    {
        string[] values = null;
        var csvFile = Resources.Load("question") as TextAsset;
        using (var sr = new StringReader(csvFile.text))
        {
            while (sr.Peek() > -1)
            {
                var line = sr.ReadLine();
                values = line.Split(',');

                if (questions.ContainsKey(int.Parse(values[6])))
                {
                    questions[int.Parse(values[6])].Add(new Question(values[0], new string[] { values[1], values[2], values[3], values[4] }, int.Parse(values[5]), int.Parse(values[6])));
                }
                else
                {
                    questions.Add(int.Parse(values[6]), new List<Question>());
                    questions[int.Parse(values[6])].Add(new Question(values[0], new string[] { values[1], values[2], values[3], values[4] }, int.Parse(values[5]), int.Parse(values[6])));
                }
            }
        }
        Debug.Log(questions[2][1].Q);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class QuestionSystem
{
    Dictionary<int, List<Question>> questions = new Dictionary<int, List<Question>>();
    public Question GetQuestion(int difficulty)
    {
        /* ここにリストから呼び出す処理　*/
        //int count = questions[difficulty].Count;
        return questions[2][1]; //Question.Null;
    }

    public void CSVRead()
    {
        string[] values = null;
        using (var sr = new StreamReader(@"question.csv"))
        {
            
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                values = line.Split(',');

                if (questions.ContainsKey(int.Parse(values[6]))){
                    questions[int.Parse(values[6])].Add(new Question(values[0], new string[] { values[1], values[2], values[3], values[4] }, int.Parse(values[5]), int.Parse(values[6])));
                }
                else
                {
                    questions.Add(int.Parse(values[6]), new List<Question>());
                    questions[int.Parse(values[6])].Add(new Question(values[0], new string[] { values[1], values[2], values[3], values[4] }, int.Parse(values[5]), int.Parse(values[6])));
                }
            }
        }
        Debug.Log("csv");
    }
}

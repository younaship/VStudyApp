using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public string Q; // 質問
    public string[] As; //　答え例 [4]
    public int A; // 答え (0-3)
    public int difficulty;

    public static Question Null {
        get { return new Question("What?",new string[] { "A","B","C","D" }, 0, 1); }
    }

    public Question(string question,string[] answers,int answerIndex,int difficulty)
    {
        this.Q = question;
        this.A = answerIndex;
        this.As = answers;
        this.difficulty = difficulty;
    }
}

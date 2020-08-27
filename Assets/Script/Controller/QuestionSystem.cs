using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionSystem
{
    Dictionary<int,List<Question>> questions;

    public Question GetQuestion(int difficulty)
    {
        /* ここにリストから呼び出す処理　*/
        //int count = questions[difficulty].Count;
        return Question.Null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionController : MonoBehaviour
{
    public QuestionSystem QuestionSystem { get; private set; }
    public void Init()
    {
        QuestionSystem = new QuestionSystem();
        QuestionSystem.CSVRead();
    }
}

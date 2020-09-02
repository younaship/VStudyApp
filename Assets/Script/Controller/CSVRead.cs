using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVRead : MonoBehaviour
{

    void Start()
    {
        using (var sr = new StreamReader(@"quesion.csv"))
        {
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                var values = line.Split(',');
            }
        }

    }

}

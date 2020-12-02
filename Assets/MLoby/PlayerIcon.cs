using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    [SerializeField] Text text;

    string id = "Unknown";

    public void Set(string name)
    {
        this.id = name;
    }

    private void Update()
    {
        this.text.text = this.id;
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }
}

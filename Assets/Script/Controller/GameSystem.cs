using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem
{
    public QuestionSystem questionSystem;
    public Player player;

    public async void Init()
    {
        player = new Player();
        player.Hp = 10;
        player.MaxHp = 15;

        questionSystem = new QuestionSystem(); // init
    }
}

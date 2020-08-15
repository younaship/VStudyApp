using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public UIController UIController;
    public SceneController SceneController;

    GameSystem gameSystem;

    public void Awake()
    {
        
    }

    public void InitGame(GameSystem gameSystem)
    {
        this.gameSystem = gameSystem;

        UIController.SetHP(gameSystem.player.Hp, gameSystem.player.MaxHp);
    }

}

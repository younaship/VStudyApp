using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStage
{
    StageInfo GetStage(int index);
}

public abstract class Stage : IStage
{
    public Stage(GameSystem gameSystem) { }
    public abstract StageInfo GetStage(int index);
}

public class StageInfo
{
    public bool IsShop;
    public Enemy Enemy;
    public Item DropItem;

    public Sprite BackImage; 

    public int MinQuestionDifficulty;
    public int MaxQuestionDifficulty;
}

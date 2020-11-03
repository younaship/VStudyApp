using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public interface IStage
{
    StageInfo GetStage(int index);
}

public class StageInfo
{
    public StageType Type = StageType.battle;
    public Enemy Enemy;
    public Item DropItem;

    public Sprite BackImage; 

    public int MinQuestionDifficulty;
    public int MaxQuestionDifficulty;
}
*/

public abstract class Stage //: IStage
{
    public Stage(GameSystem gameSystem) { }
    //public abstract StageInfo GetStage(int index);
    public abstract Round GetRound(int index);
}


public class Round
{
    public StageType Type;
    public Sprite BackImage;
}

public class ButtleRound : Round
{
    public ButtleRound()
    {
        this.Type = StageType.battle;
    }

    public Enemy Enemy;
    public Item DropItem;

    public int MinQuestionDifficulty;
    public int MaxQuestionDifficulty;
}

public class ShopRound : Round
{
    public ShopRound()
    {
        this.Type = StageType.shop;
    }
    
    public Item item;
    public Sprite GetShopImage()
    {
        if(item is Weapon)
            return Resources.Load<Sprite>("shop_w");
        if(item is Armor)
            return Resources.Load<Sprite>("shop_a");

        return Resources.Load<Sprite>("");
    }

}

public enum StageType
{
    battle, shop
}
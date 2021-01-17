using UnityEngine;

public class SampleStage : Stage
{

    Round stageInfo;

    public SampleStage(GameSystem gameSystem) : base(gameSystem)
    {
        /*
        this.stageInfo = new StageInfo()
        {
            MinQuestionDifficulty = 1,
            MaxQuestionDifficulty = 2,
            Enemy = new Enemy()
            {
                Atk = 3,
                AttackRate = 1.0f,
                MaxHp = 20,
                Hp = 20,
                Normal = Resources.Load<Sprite>("enemy_normal")
            },
            BackImage = Resources.Load<Sprite>("bg_normal")
        }; /* Debug */
    }

    public override Round GetRound(int index)
    {

        Sprite bgimage = Resources.Load<Sprite>("bg_normal");

        if (index == 1) return new ShopRound()
        {
            BackImage = bgimage,
            Item = new Weapon("BUKI", 50, 50, 10, "weapon/W_2")
        };

        if (index == 3) return new ShopRound()
        {
            BackImage = bgimage,
            Item = new Weapon("BUKI 2", 100, 100, 20, "weapon/W_3")
        };

        return new ButtleRound()
        {
            MinQuestionDifficulty = 1,
            MaxQuestionDifficulty = 1,
            BackImage = bgimage,
            DropItem = new Money(100),
            Enemy = new Enemy()
            {
                Atk = 1,
                AttackRate = 10.0f,
                MaxHp = 20,
                Hp = 20,
                Normal = Resources.Load<Sprite>("enemy_normal")
            },
        };

    }
}
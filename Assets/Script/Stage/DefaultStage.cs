using UnityEngine;

/*
public class DefaultStage : Stage
{

    StageInfo stageInfo;

    public DefaultStage(GameSystem gameSystem) : base(gameSystem)
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
        }; // Debug 
    }

    public override StageInfo GetStage(int index)
    {
        if(index == 1) return new StageInfo()
        {
            Type = StageType.shop,
            MinQuestionDifficulty = 1,
            MaxQuestionDifficulty = 2,
            Enemy = new Enemy()
            {
                Atk = 3,
                AttackRate = 2.0f,
                MaxHp = 20,
                Hp = 20,
                Normal = Resources.Load<Sprite>("enemy_normal")
            },
            BackImage = Resources.Load<Sprite>("bg_normal")
        };

        return new StageInfo()
        {
            MinQuestionDifficulty = 1,
            MaxQuestionDifficulty = 2,
            Enemy = new Enemy()
            {
                Atk = 3,
                AttackRate = 2.0f,
                MaxHp = 20,
                Hp = 20,
                Normal = Resources.Load<Sprite>("enemy_normal")
            },
            BackImage = Resources.Load<Sprite>("bg_normal")
        };
    }
}
*/
public class DefaultStage : Stage
{

    Round stageInfo;

    public DefaultStage(GameSystem gameSystem) : base(gameSystem)
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
        if (index == 0) return new ShopRound()
        {
            item = new Weapon("BUKI", 100, 120, 5),
            BackImage = Resources.Load<Sprite>("bg_normal")
        };

        return new ButtleRound()
        {
            MinQuestionDifficulty = 1,
            MaxQuestionDifficulty = 2,
            Enemy = new Enemy()
            {
                Atk = 3,
                AttackRate = 2.0f,
                MaxHp = 20,
                Hp = 20,
                Normal = Resources.Load<Sprite>("enemy_normal")
            },
            BackImage = Resources.Load<Sprite>("bg_normal")
        };
    }
}
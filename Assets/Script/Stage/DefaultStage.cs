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
/*
public class DefaultStage : Stage
{

    Round stageInfo;

    public DefaultStage(GameSystem gameSystem) : base(gameSystem)
    {
        *//*
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
/*    }*/
    /*
    public override Round GetRound(int index)
    {

        Sprite bgimage;
        if (index < 2) bgimage = Resources.Load<Sprite>("bg_normal");
        else if (index < 4) bgimage = Resources.Load<Sprite>("bg_normal_e");
        else if (index < 6) bgimage = Resources.Load<Sprite>("bg_normal_n");
        else bgimage = Resources.Load<Sprite>("bg_normal");

        if (index == 1) return new ShopRound()
        {
            item = new Weapon("BUKI", 100, 120, 5),
            BackImage = bgimage
        };

        if (index == 5) return new ShopRound()
        {
            item = new Weapon("BUKI2", 1000, 1200, 50),
            BackImage = bgimage
        };

        if (index == 3) return new ShopRound()
        {
            item = new Armor("あーまー１", 100, 80, 10),
            BackImage = bgimage
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
                Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 20))
            },
            BackImage = bgimage
        };
    }
}*/
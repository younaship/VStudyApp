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
public class HegiStage : Stage
{

    Round stageInfo;

    public HegiStage(GameSystem gameSystem) : base(gameSystem)
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

        Sprite bgimage;
        if (index % 30 < 11) bgimage = Resources.Load<Sprite>("bg_normal");
        else if (index % 30 < 21) bgimage = Resources.Load<Sprite>("bg_normal_e");
        else if (index % 30 < 31) bgimage = Resources.Load<Sprite>("bg_normal_n");
        else bgimage = Resources.Load<Sprite>("bg_normal");

        if (index >0 &&
            index % 10 == 0)
        {
            float plus = index * 0.1f;
            if (Random.value < 0.5)
            {
                return new ShopRound()
                {
                    item = new Weapon("HegiTestWep", 100, 120 + plus / 120, 5 + plus / 5),
                    BackImage = bgimage
                };
            }
            else
            {
                return new ShopRound()
                {
                    item = new Armor("HegiTestArm", 100, 80 + plus / 80, 10 + plus / 10),
                    BackImage = bgimage
                };
            }
        }

        if (index < 20)
        {
            float plus = index * 0.1f;
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 1,
                Enemy = new Enemy()
                {
                    Atk = 3 + plus / 3,
                    AttackRate = 2.0f + plus / 2.0f,
                    MaxHp = 20 + plus / 20,
                    Hp = 20 + plus / 20,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 20))
                },
                BackImage = bgimage
            };
        }
        else if (index < 40)
        {
            float plus = index * 0.2f;
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 1,
                Enemy = new Enemy()
                {
                    Atk = 3 + plus / 3,
                    AttackRate = 2.0f + plus / 2.0f,
                    MaxHp = 20 + plus / 20,
                    Hp = 20 + plus / 20,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 20))
                },
                BackImage = bgimage
            };
        }
        else if (index < 60)
        {
            float plus = index * 0.3f;
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 1,
                Enemy = new Enemy()
                {
                    Atk = 3 + plus / 3,
                    AttackRate = 2.0f + plus / 2.0f,
                    MaxHp = 20 + plus / 20,
                    Hp = 20 + plus / 20,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 20))
                },
                BackImage = bgimage
            };
        }
        else
        {
            float plus = index * 0.4f;
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 1,
                Enemy = new Enemy()
                {
                    Atk = 3 + plus / 3,
                    AttackRate = 2.0f + plus / 2.0f,
                    MaxHp = 20 + plus / 20,
                    Hp = 20 + plus / 20,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 20))
                },
                BackImage = bgimage
            };
        }
    }
}
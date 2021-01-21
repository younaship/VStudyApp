using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayStage : Stage
{
    public TestPlayStage(GameSystem gameSystem) : base(gameSystem)
    { 
    }

    public override Round GetRound(int index)
    {
        Sprite bgimage;
        if (index % 30 < 11) bgimage = Resources.Load<Sprite>("bg_normal");
        else if (index % 30 < 21) bgimage = Resources.Load<Sprite>("bg_normal_e");
        else if (index % 30 < 31) bgimage = Resources.Load<Sprite>("bg_normal_n");
        else bgimage = Resources.Load<Sprite>("bg_normal");


        if (index > 0 && index % 2 == 0)
        {
            float plus = index * 0.1f;

            string name = "Made in Stage" + index;
            int price = index * 10 + Random.Range(-index, index);


 

            if (Random.value < 0.5)
            {
                float power = 5 + plus / 5;
                string spritePath = "weapon/W_" + 1;

                return new ShopRound()
                {
                    Item = new Weapon(name, price, price, power, spritePath),
                    BackImage = bgimage
                };
            }
            else
            {
                float difence = 10 + plus / 10;
                string spritePath = "armor/A_" + 1;

                return new ShopRound()
                {
                    Item = new Armor(name, price, price, difence, spritePath),
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
                BackImage = bgimage,
                DropItem = new Money(1000/*Random.value < 0.7 ? 1 : 0*/)
            };
        }
        else if (index < 40)
        {
            float plus = index * 0.2f;
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 2,
                Enemy = new Enemy()
                {
                    Atk = 3 + plus / 3,
                    AttackRate = 2.0f + plus / 2.0f,
                    MaxHp = 20 + plus / 20,
                    Hp = 20 + plus / 20,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 20))
                },
                BackImage = bgimage,
                DropItem = new Money(Random.value < 0.7 ? 2 : 1)
            };
        }
        else if (index < 60)
        {
            float plus = index * 0.3f;
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 3,
                Enemy = new Enemy()
                {
                    Atk = 3 + plus / 3,
                    AttackRate = 2.0f + plus / 2.0f,
                    MaxHp = 20 + plus / 20,
                    Hp = 20 + plus / 20,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 20))
                },
                BackImage = bgimage,
                DropItem = new Money(Random.value < 0.7 ? 3 : 2)
            };
        }
        else
        {
            float plus = index * 0.4f;
            return new ButtleRound()
            {
                MinQuestionDifficulty = 2,
                MaxQuestionDifficulty = 3,
                Enemy = new Enemy()
                {
                    Atk = 3 + plus / 3,
                    AttackRate = 2.0f + plus / 2.0f,
                    MaxHp = 20 + plus / 20,
                    Hp = 20 + plus / 20,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 20))
                },
                BackImage = bgimage,
                DropItem = new Money(Random.value < 0.7 ? 4 : 3)
            };
        }

    }
}

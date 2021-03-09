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


        if (index > 0 && index % 5 == 0)
        {
            int price;
            float power;
            float difence;
            if (index <= 20)
            {
                price = 75;
                power = Random.Range(50, 101);
                difence = Random.Range(5, 7);

            }
            else if (index <= 40)
            {
                price = 285;
                power = Random.Range(150, 301);
                difence = Random.Range(7, 11);
            }
            else if (index <= 60)
            {
                price = 1065;
                power = Random.Range(400, 801);
                difence = Random.Range(15, 31);
            }
            else
            {
                price = 3940;
                power = Random.Range(1000, 5001);
                difence = Random.Range(50, 101);
            }

            string name = "強さ" + index / 5 + "の";
            if (Random.value < 0.5)
            {
                name += "武器";
                return new ShopRound()
                {
                    Item = new Weapon(
                        name,
                        price,
                        price,
                        power,
                        "weapon/W_" + Random.Range(1, 21)
                        ),
                    BackImage = bgimage
                };
            }
            else
            {
                name += "防具";
                return new ShopRound()
                {
                    Item = new Armor(
                        name,
                        price,
                        price,
                        difence,
                        "armor/A_" + Random.Range(1, 21)
                        ),
                    BackImage = bgimage
                };
            }
        }


        if (index <= 20)
        {
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 2,
                Enemy = new Enemy()
                {
                    Atk = 10,
                    AttackRate = 5,
                    MaxHp = 150,
                    Hp = 150,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 21))
                },
                BackImage = bgimage,
                DropItem = new Money(Random.Range(5, 11))
            };
        }
        else if (index <= 40)
        {
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 2,
                Enemy = new Enemy()
                {
                    Atk = 20,
                    AttackRate = 5,
                    MaxHp = 300,
                    Hp = 300,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 21))
                },
                BackImage = bgimage,
                DropItem = new Money(Random.Range(20, 36))
            };
        }
        else if (index <= 60)
        {
            float plus = index * 0.3f;
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 2,
                Enemy = new Enemy()
                {
                    Atk = 40,
                    AttackRate = 5,
                    MaxHp = 1000,
                    Hp = 1000,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 21))
                },
                BackImage = bgimage,
                DropItem = new Money(Random.Range(50, 101))
            };
        }
        else
        {
            return new ButtleRound()
            {
                MinQuestionDifficulty = 1,
                MaxQuestionDifficulty = 2,
                Enemy = new Enemy()
                {
                    Atk = 100,
                    AttackRate = 5,
                    MaxHp = 5000,
                    Hp = 5000,
                    Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 21))
                },
                BackImage = bgimage,
                DropItem = new Money(Random.Range(200, 501))
            };
        }

    }
}

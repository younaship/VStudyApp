using UnityEngine;


public class MultiStage : Stage
{

    Round stageInfo;

    public MultiStage(GameSystem gameSystem) : base(gameSystem) { }

    public override Round GetRound(int index)
    {

        Sprite bgimage;
        if (index < 5) bgimage = Resources.Load<Sprite>("bg_normal");
        else if (index < 10) bgimage = Resources.Load<Sprite>("bg_normal_e");
        else if (index < 15) bgimage = Resources.Load<Sprite>("bg_normal_n");
        else bgimage = Resources.Load<Sprite>("bg_normal");

        var plus = 10 * index;

        return new ButtleRound()
        {
            MinQuestionDifficulty = 1,
            MaxQuestionDifficulty = 1,
            Enemy = new Enemy()
            {
                Atk = 3 + plus / 2,
                AttackRate = 2.0f,
                MaxHp = 20 + plus ,
                Hp = 20 + plus,
                Normal = Resources.Load<Sprite>("enemy/E_" + Random.Range(1, 20))
            },
            BackImage = bgimage
        };
    }
}
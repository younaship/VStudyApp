﻿using UnityEngine;

public class DefaultStage : Stage
{

    StageInfo stageInfo;

    public DefaultStage(GameSystem gameSystem) : base(gameSystem)
    {
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

    public override StageInfo GetStage(int index)
    {
        return this.stageInfo;
    }
}
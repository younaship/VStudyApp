public class DefaultStage : Stage
{
    public override StageInfo GetStage(int index)
    {
        return new StageInfo() {
            MinQuestionDifficulty = 1,
            MaxQuestionDifficulty = 2,
            Enemy = new Enemy()
            {
                Atk = 3,
                AttackRate = 1.0f,
                MaxHp = 20,
            }
        }; /* Debug */
    }
}
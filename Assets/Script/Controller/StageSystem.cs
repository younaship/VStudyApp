public class StageSystem
{
    public bool IsReady { get { if (this.CurrentStage is null) return true; else return false; } }
    public Stage CurrentStage { get; private set; }

    public StageSystem()
    {
        this.CurrentStage = new DefaultStage();
    }

    public void SetCurrentStage(Stage stage)
    {
        CurrentStage = stage;
    }
}

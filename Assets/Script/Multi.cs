using System;

public class GameMode
{
    public GameMode(Mode mode) { this.Value = mode; }
    public Mode Value { get; private set; }
    public enum Mode
    {
        Single, Multi
    }
}

public class MultiResult
{
    public int Score;
}
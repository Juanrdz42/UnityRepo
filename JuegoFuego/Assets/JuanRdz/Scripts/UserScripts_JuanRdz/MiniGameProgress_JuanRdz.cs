using System;

[System.Serializable]
public class MiniGameProgress_JuanRdz
{
    public string miniGameId;
    public bool unlocked;
    public bool completed;
    public int bestScore;
    public float bestTime;
    public int timesPlayed;

    public MiniGameProgress_JuanRdz(string id, bool startsUnlocked = false)
    {
        miniGameId = id;
        unlocked = startsUnlocked;
        completed = false;
        bestScore = 0;
        bestTime = 0f;
        timesPlayed = 0;
    }

    public void RegisterPlay(int score, float time, bool won)
    {
        timesPlayed++;

        if (score > bestScore)
            bestScore = score;

        if (bestTime <= 0f || time < bestTime)
            bestTime = time;

        if (won)
            completed = true;
    }
}
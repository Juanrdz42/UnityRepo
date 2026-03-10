using System.Collections.Generic;

[System.Serializable]
public class UserData_JuanRdz
{
    public string userId;
    public string playerName;

    public int totalScore;
    public int totalPhotosTaken;
    public int totalPerfectPhotos;
    public int totalGoodPhotos;
    public int totalBadPhotos;

    public Dictionary<string, MiniGameProgress_JuanRdz> miniGames =
        new Dictionary<string, MiniGameProgress_JuanRdz>();

    public UserData_JuanRdz(string id, string name)
    {
        userId = id;
        playerName = name;
        totalScore = 0;
        totalPhotosTaken = 0;
        totalPerfectPhotos = 0;
        totalGoodPhotos = 0;
        totalBadPhotos = 0;
    }

    public void AddMiniGame(string miniGameId, bool startsUnlocked = false)
    {
        if (!miniGames.ContainsKey(miniGameId))
        {
            miniGames.Add(miniGameId, new MiniGameProgress_JuanRdz(miniGameId, startsUnlocked));
        }
    }

    public bool IsMiniGameUnlocked(string miniGameId)
    {
        return miniGames.ContainsKey(miniGameId) && miniGames[miniGameId].unlocked;
    }

    public bool IsMiniGameCompleted(string miniGameId)
    {
        return miniGames.ContainsKey(miniGameId) && miniGames[miniGameId].completed;
    }

    public void UnlockMiniGame(string miniGameId)
    {
        if (miniGames.ContainsKey(miniGameId))
        {
            miniGames[miniGameId].unlocked = true;
        }
    }

    public void RegisterPhotoResults(int score, int perfect, int good, int bad)
    {
        totalScore += score;
        totalPerfectPhotos += perfect;
        totalGoodPhotos += good;
        totalBadPhotos += bad;
        totalPhotosTaken += perfect + good + bad;
    }

    public void RegisterMiniGameResult(string miniGameId, int score, float time, bool won)
    {
        if (!miniGames.ContainsKey(miniGameId))
            return;

        miniGames[miniGameId].RegisterPlay(score, time, won);
    }
}
using UnityEngine;

public class UserManager_JuanRdz : MonoBehaviour
{
    public static UserManager_JuanRdz Instance;

    public UserData_JuanRdz currentUser;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeUser();
    }

    private void InitializeUser()
    {
        currentUser = new UserData_JuanRdz("user_001", "Jugador");

        currentUser.AddMiniGame("Mini2_Bosque", true);
        currentUser.AddMiniGame("Mini2_Lago", true);
        currentUser.AddMiniGame("Mini3", false);
        currentUser.AddMiniGame("Mini4", false);
    }

    public void RegisterMiniGameWin(string miniGameId, int score, float time, int perfect, int good, int bad)
    {
        currentUser.RegisterPhotoResults(score, perfect, good, bad);
        currentUser.RegisterMiniGameResult(miniGameId, score, time, true);

        CheckUnlocks(miniGameId);
    }

    public void RegisterMiniGameLoss(string miniGameId, int score, float time, int perfect, int good, int bad)
    {
        currentUser.RegisterPhotoResults(score, perfect, good, bad);
        currentUser.RegisterMiniGameResult(miniGameId, score, time, false);
    }

    private void CheckUnlocks(string completedMiniGameId)
    {
        if (completedMiniGameId == "Mini2_Bosque" || completedMiniGameId == "Mini2_Lago")
        {
            currentUser.UnlockMiniGame("Mini3");
            currentUser.UnlockMiniGame("Mini4");
        }
    }
}
using UnityEngine;

public class QuestionPanelManager : MonoBehaviour
{
    public void ClosePanel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f; // reanuda el juego
    }

    public void CorrectAnswer()
    {
        // gana regaderas
        ClosePanel();
    }

    public void WrongAnswer()
    {
        // pierde regaderas
        ClosePanel();
    }
}

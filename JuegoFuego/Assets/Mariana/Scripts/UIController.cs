using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    int time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // el juego tiene límite de tiempo
        time = GameControl.Instance.timeToWin;
        ActiveText();
    }

    public void ActiveText()
    {
        timeText.text = time.ToString();
    }

    public void StartTimer()
    {
        StartCoroutine(MatchTime());
    }

    IEnumerator MatchTime()
    {
        // temporizador
        yield return new WaitForSeconds(1);
        time -= 1;
        ActiveText();
        if (time == 0) // se termina
            SceneManager.LoadScene("endScene");
        else
            StartCoroutine(MatchTime());
    }
    void Update()
    {
        
    }
}
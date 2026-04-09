using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip good;
    public AudioClip bad;
    public AudioClip question;
    public AudioClip win; 
    public AudioClip lose; 

    public void GoodSound()
    {
        AudioSource.PlayClipAtPoint(good, Camera.main.transform.position, 0.8f); 
    }
    public void BadSound()
    {
        AudioSource.PlayClipAtPoint(bad, Camera.main.transform.position, 0.5f); // volumen al 50
    }
    public void QuestionSound()
    {
        AudioSource.PlayClipAtPoint(question, Camera.main.transform.position, 0.5f); // volumen al 50
    }
    public void WinSound()
    {
        AudioSource.PlayClipAtPoint(win, Camera.main.transform.position, 0.25f); // audio para la escena final
    }
    public void LoseSound()
    {
        AudioSource.PlayClipAtPoint(lose, Camera.main.transform.position, 0.25f); // audio para la escena final

    }
}
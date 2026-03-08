using UnityEngine;
using TMPro;

public class PhotoController : MonoBehaviour
{
    public static PhotoController Instance;

    public GameObject photoGamePanel;
    public TMP_Text resultText;

    private PhotoSpot currentSpot;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (photoGamePanel != null)
            photoGamePanel.SetActive(false);
    }

    public void OpenPhotoGame(PhotoSpot spot)
    {
        currentSpot = spot;

        if (photoGamePanel != null)
            photoGamePanel.SetActive(true);

        if (resultText != null)
            resultText.text = "¡Toma la foto!";
    }

    public void CompletePhoto()
    {
        if (currentSpot != null)
        {
            currentSpot.CompleteSpot();
            currentSpot = null;
        }

        if (photoGamePanel != null)
            photoGamePanel.SetActive(false);
    }
}
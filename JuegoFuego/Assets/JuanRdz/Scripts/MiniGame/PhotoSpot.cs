using UnityEngine;

public class PhotoSpot : MonoBehaviour, IInteractable
{
    public GameObject birdVisual;
    public bool isActiveSpot = false;
    public bool isCompleted = false;

    private void Start()
    {
        SetActiveSpot(false);
    }

    public void SetActiveSpot(bool active)
    {
        isActiveSpot = active;
        isCompleted = false;

        if (birdVisual != null)
            birdVisual.SetActive(active);
    }

    public void CompleteSpot()
    {
        isCompleted = true;
        isActiveSpot = false;

        if (birdVisual != null)
            birdVisual.SetActive(false);
    }

    public bool CanInteract()
    {
        return isActiveSpot && !isCompleted;
    }

    public void Interact()
    {
        if (!CanInteract())
            return;

        Debug.Log("Abrir minijuego de fotografía");

        if (PhotoController.Instance != null)
        {
            PhotoController.Instance.OpenPhotoGame(this);
        }
    }
}
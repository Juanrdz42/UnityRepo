using UnityEngine;

public class PhotoSpot : MonoBehaviour, IInteractable
{
    public GameObject spotVisual;
    public bool isActiveSpot = false;
    public bool isCompleted = false;
    public PhotoSequenceData sequenceData;

    private void Start()
    {
        SetActiveSpot(false);
    }

    public void SetActiveSpot(bool active)
    {
        isActiveSpot = active;
        isCompleted = false;

        if (spotVisual != null)
            spotVisual.SetActive(active);
    }

    public void CompleteSpot()
    {
        isCompleted = true;
        isActiveSpot = false;

        if (spotVisual != null)
            spotVisual.SetActive(false);
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
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector_JuanRdz : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    public GameObject interactionIcon;

    void Start()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // Si la cámara está abierta, Z toma la foto
        if (PhotoController.Instance != null && PhotoController.Instance.IsPhotoGameOpen())
        {
            PhotoController.Instance.TryTakePhoto();

            if (interactionIcon != null)
                interactionIcon.SetActive(false);

            return;
        }

        // Si no, interacción normal
        if (interactableInRange != null && interactableInRange.CanInteract())
        {
            interactableInRange.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && interactable.CanInteract())
        {
            interactableInRange = interactable;

            if (interactionIcon != null && !IsPhotoGameOpen())
                interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && interactableInRange == interactable)
        {
            interactableInRange = null;

            if (interactionIcon != null)
                interactionIcon.SetActive(false);
        }
    }

    private bool IsPhotoGameOpen()
    {
        return PhotoController.Instance != null && PhotoController.Instance.IsPhotoGameOpen();
    }
}
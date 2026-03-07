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
        if (context.performed && interactableInRange != null && interactableInRange.CanInteract())
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

            if (interactionIcon != null)
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
}

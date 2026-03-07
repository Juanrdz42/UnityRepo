using UnityEngine;

public class Map_JuanRdz : MonoBehaviour, IInteractable
{
    public GameObject mapPanel;

    public void Interact()
    {
        mapPanel.SetActive(true);
    }

    public bool CanInteract()
    {
        return true;
    }
}
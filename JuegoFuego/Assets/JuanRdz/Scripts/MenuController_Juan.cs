using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController_Juan : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject instructionsPanel;

    [Header("Player")]
    public PlayerMove playerMovement;

    private Rigidbody2D playerRb;

    private void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        if (playerMovement != null)
            playerRb = playerMovement.GetComponent<Rigidbody2D>();

        UpdatePlayerMovementState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (menuPanel == null) return;

        bool newState = !menuPanel.activeSelf;
        menuPanel.SetActive(newState);

        if (!newState && instructionsPanel != null)
            instructionsPanel.SetActive(false);

        UpdatePlayerMovementState();
    }

    public void OpenMenu()
    {
        if (menuPanel != null)
            menuPanel.SetActive(true);

        UpdatePlayerMovementState();
    }

    public void CloseMenu()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        UpdatePlayerMovementState();
    }

    public void OpenInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);

        UpdatePlayerMovementState();
    }

    public void CloseInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        UpdatePlayerMovementState();
    }

    public void GoToMap()
    {
        SceneManager.LoadScene("Map");
    }

    private void UpdatePlayerMovementState()
    {
        bool uiOpen = false;

        if (menuPanel != null && menuPanel.activeSelf)
            uiOpen = true;

        if (instructionsPanel != null && instructionsPanel.activeSelf)
            uiOpen = true;

        if (playerMovement != null)
            playerMovement.enabled = !uiOpen;

        if (uiOpen && playerRb != null)
            playerRb.linearVelocity = Vector2.zero;
    }
}
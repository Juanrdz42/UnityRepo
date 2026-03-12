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

        bool shouldOpenInstructions = false;
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Mini2")
        {
            if (QuestController_JuanRdz.Instance == null ||
                !QuestController_JuanRdz.Instance.IsPostGameActive())
            {
                shouldOpenInstructions = true;
            }
        }

        if (shouldOpenInstructions && instructionsPanel != null)
            instructionsPanel.SetActive(true);

        UpdateUIState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMenu();
    }

    public void ToggleMenu()
    {
        if (menuPanel == null) return;

        bool newState = !menuPanel.activeSelf;
        menuPanel.SetActive(newState);

        if (!newState && instructionsPanel != null)
            instructionsPanel.SetActive(false);

        UpdateUIState();
    }

    public void OpenMenu()
    {
        if (menuPanel != null)
            menuPanel.SetActive(true);

        UpdateUIState();
    }

    public void CloseMenu()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        UpdateUIState();
    }

    public void OpenInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);

        UpdateUIState();
    }

    public void CloseInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        UpdateUIState();
    }

    public void GoToMap()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Map");
    }

    private void UpdateUIState()
    {
        bool uiOpen = false;

        if (menuPanel != null && menuPanel.activeSelf)
            uiOpen = true;

        if (instructionsPanel != null && instructionsPanel.activeSelf)
            uiOpen = true;

        if (playerMovement != null)
            playerMovement.SetMovementEnabled(!uiOpen);

        if (uiOpen && playerRb != null)
            playerRb.linearVelocity = Vector2.zero;

        if (ShouldPauseGameTime())
            Time.timeScale = uiOpen ? 0f : 1f;
        else
            Time.timeScale = 1f;
    }

    private bool ShouldPauseGameTime()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return sceneName == "Mini2_Bosque" || sceneName == "Mini2_Lago";
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
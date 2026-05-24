using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int levelNumber = 1;

    [Header("Scene References")]
    [SerializeField] private BurgerAssemblyPlace burgerAssemblyPlace;
    [SerializeField] private BunDispenser bunDispenser;

    [Header("Result Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    
    [Header("Gameplay Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerInteraction playerInteraction;

    private bool resultShown;

    private void Start()
    {
        Time.timeScale = 1f;
        
        LockCursorForGameplay();
        SetGameplayScriptsEnabled(true);
        
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        resultShown = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartLevel(levelNumber);
        }
        else
        {
            Debug.LogError("LevelManager: GameManager.Instance не найден.");
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null || resultShown)
        {
            return;
        }

        if (!GameManager.Instance.LevelEnded)
        {
            return;
        }

        resultShown = true;
        
        switch (GameManager.Instance.CurrentResult)
        {
            case LevelResult.Win:
                ShowWinPanel();
                break;

            case LevelResult.Lose:
                ShowLosePanel();
                break;

            default:
                Debug.LogWarning("LevelManager: уровень завершён, но результат не указан.");
                break;
        }
    }

    public void PrepareNextBurger()
    {
        if (burgerAssemblyPlace != null)
        {
            burgerAssemblyPlace.ClearBurger();
        }

        if (bunDispenser != null)
        {
            bunDispenser.ResetBunOrder();
        }
    }

    public void ShowWinPanel()
    {
        ShowResultPanel(winPanel);
        Debug.Log("Показана панель победы.");
    }

    public void ShowLosePanel()
    {
        ShowResultPanel(losePanel);
        Debug.Log("Показана панель поражения.");
    }
    
    private void ShowResultPanel(GameObject panel)
    {
        Time.timeScale = 0f;

        UnlockCursorForUI();
        SetGameplayScriptsEnabled(false);

        if (panel != null)
        {
            panel.SetActive(true);
        }
        else
        {
            Debug.LogError("LevelManager: панель результата не назначена.");
        }
    }
    
    private void SetGameplayScriptsEnabled(bool value)
    {
        if (playerController != null)
        {
            playerController.enabled = value;
        }

        if (cameraController != null)
        {
            cameraController.enabled = value;
        }

        if (playerInteraction != null)
        {
            playerInteraction.enabled = value;
        }
    }

    private void LockCursorForGameplay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursorForUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    // --- Buttons --- //

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
        
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }
}
using UnityEngine;

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

    private bool resultShown;

    private void Start()
    {
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
        if (GameManager.Instance == null || resultShown == true)
        {
            return;
        }

        if (!GameManager.Instance.LevelEnded)
        {
            return;
        }

        resultShown = true;
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
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    public void ShowLosePanel()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
    }
}
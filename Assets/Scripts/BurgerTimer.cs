using TMPro;
using UnityEngine;

public class BurgerTimer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    private float timeLeft;
    private bool isRunning;

    private int lastShownSecond = -1;

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.LevelEnded)
        {
            StopTimer();
            return;
        }

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            UpdateTimerText();

            isRunning = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseLevel();
            }

            return;
        }

        UpdateTimerText();
    }

    public void StartTimer()
    {
        timeLeft = GameSettings.TimePerBurger;
        isRunning = true;
        lastShownSecond = -1;

        UpdateTimerText();

        Debug.Log("Таймер запущен: " + GameSettings.TimePerBurger + " сек.");
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
        {
            return;
        }

        int seconds = Mathf.CeilToInt(timeLeft);

        if (seconds == lastShownSecond)
        {
            return;
        }

        lastShownSecond = seconds;
        timerText.text = "0:" + seconds;
    }
}
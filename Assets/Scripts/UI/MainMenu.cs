using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("2");
    }

    public void OpenSettings()
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
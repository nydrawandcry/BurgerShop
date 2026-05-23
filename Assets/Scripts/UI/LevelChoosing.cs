using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelChoosing : MonoBehaviour
{
    public static int _previousSceneIndex = 0;
    
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ReturnToPreviousScene();
        }
    }
    
    public void StartLevel1()
    {
        SceneManager.LoadSceneAsync(3);
    }
    
    public void StartLevel2()
    {
        SceneManager.LoadSceneAsync(4);
    }
    
    public void StartLevel3()
    {
        SceneManager.LoadSceneAsync(5);
    }
    
    public void ReturnToPreviousScene()
    {
        SceneManager.LoadScene(_previousSceneIndex);
    }
}
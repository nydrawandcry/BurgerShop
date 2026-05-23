using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public static int _previousSceneIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToPreviousScene();
        }
    }
    
    public void ReturnToPreviousScene()
    {
        SceneManager.LoadScene(_previousSceneIndex);
    }
}
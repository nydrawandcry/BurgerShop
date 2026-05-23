using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChoosing : MonoBehaviour
{
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
}
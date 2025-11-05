using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndButtons : MonoBehaviour
{
    public void ReloadScene()
    {
        ObjectScript.gameEnded = false;
        Time.timeScale = 1f;  
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        ObjectScript.gameEnded = false;
        Time.timeScale = 1f;  
        SceneManager.LoadScene(0);
    }
}
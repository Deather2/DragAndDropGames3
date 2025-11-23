using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject losePanel;
    public bool gameOver = false;

    void Awake()
    {
        Instance = this;
    }

    public void Lose()
    {
        if (gameOver) return;
        gameOver = true;

        Time.timeScale = 0f;
        losePanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

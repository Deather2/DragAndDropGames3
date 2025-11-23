using UnityEngine;
using UnityEngine.SceneManagement;
using static BlockSpawner;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance;

    public Tower[] towers;
    public int totalBlocks;

    public GameObject win1;
    public GameObject win2;
    public GameObject win3;
    public UnityEngine.UI.Text timeText1;
    public UnityEngine.UI.Text timeText2;
    public UnityEngine.UI.Text timeText3;

    public float timer;
    public bool finished = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!finished)
            timer += Time.deltaTime;
    }

    public void TryWin()
    {
        foreach (var tower in towers)
        {
            if (tower.blocks.Count == totalBlocks)
            {
                if (IsCorrectOrder(tower))
                {
                    Win();
                    return;
                }
            }
        }
    }

    bool IsCorrectOrder(Tower tower)
    {
        for (int i = 0; i < tower.blocks.Count - 1; i++)
        {
            if (tower.blocks[i].size < tower.blocks[i + 1].size)
                return false;
        }
        return true;
    }

    void Win()
    {
        finished = true;
        Time.timeScale = 0f;

        string formatted = FormatTime(timer);

        if (timer < 30f)
        {
            win3.SetActive(true);
            timeText3.text = formatted;
        }
        else if (timer < 60f)
        {
            win2.SetActive(true);
            timeText2.text = formatted;
        }
        else
        {
            win1.SetActive(true);
            timeText1.text = formatted;
        }
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);

        return $"{minutes:00}:{seconds:00}";
    }



    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

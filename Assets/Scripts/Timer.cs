using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float elapsedTime { get; private set; }

    void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int hours = Mathf.FloorToInt(elapsedTime / 3600);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00} : {1:00} : {2:00}", hours, minutes, seconds);
        }
        else
        {
            Debug.LogError("TimerText is null! Assign a Text component in the Inspector.");
        }
    }

    public void SubtractTime(float seconds)
    {
        elapsedTime = Mathf.Max(0, elapsedTime - seconds);
        UpdateTimerDisplay(); 
        Debug.Log($"Time subtracted. New elapsed time: {elapsedTime}");
    }

    public void StopTimer()
    {
        enabled = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        enabled = true;
        if (timerText != null)
        {
            timerText.text = "00 : 00 : 00";
        }
        else
        {
            Debug.LogError("TimerText is null on Reset! Assign a Text component in the Inspector.");
        }
    }
}
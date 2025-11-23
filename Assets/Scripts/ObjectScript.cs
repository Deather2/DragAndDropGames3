using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectScript : MonoBehaviour
{
    public GameObject[] vehicles;

    [HideInInspector]
    public Vector2[] startCoordinates;

    public Canvas can;
    public AudioSource effects;
    public AudioClip[] audioCli;

    [HideInInspector]
    public bool rightPlace = false;

    public static GameObject lastDragged = null;
    public static bool drag = false;
    public static bool gameEnded = false;

    private bool[] isPlaced;
    private int placedCount = 0;

    public GameObject victoryPanel;
    public GameObject defeatPanel;

    private Timer timer;

    public Image[] stars;
    public TMP_Text victoryTimeText;

    public GameObject rewardedButton;
    public GameObject bannerButton;
    public GameObject interstitialButton;

    void Awake()
    {
        timer = GetComponent<Timer>();
        if (timer == null)
        {
            Debug.LogError("Timer component not found on Script Holder!");
        }
    }

    public void InitializePositions()
    {
        if (vehicles == null || vehicles.Length == 0)
        {
            Debug.LogError("Vehicles не установлены!");
            return;
        }

        isPlaced = new bool[vehicles.Length];
        startCoordinates = new Vector2[vehicles.Length];

        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i] != null)
            {
                startCoordinates[i] = vehicles[i].GetComponent<RectTransform>().localPosition;
                Debug.Log($"Start pos for {vehicles[i].name}: {startCoordinates[i]}");
            }
            else
            {
                Debug.LogError($"Vehicle {i} is null!");
            }
        }
    }

    public void PlaceVehicle(int index)
    {
        if (index < 0 || index >= vehicles.Length)
            return;

        isPlaced[index] = true;
        placedCount++;

        if (placedCount >= vehicles.Length)
        {
            ShowVictory();
        }
    }

    private void ShowVictory()
    {
        if (timer != null)
        {
            timer.StopTimer();

            float time = timer.elapsedTime;
            int starCount = 0;

            if (time < 240f)
                starCount = 3;
            else if (time < 360f)
                starCount = 2;
            else if (time < 480f)
                starCount = 1;

            for (int i = 0; i < stars.Length; i++)
                stars[i].enabled = (i < starCount);

            int hours = Mathf.FloorToInt(time / 3600);
            int minutes = Mathf.FloorToInt((time % 3600) / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            if (victoryTimeText != null)
                victoryTimeText.text = string.Format("Time: {0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }

        if (rewardedButton != null)
            rewardedButton.SetActive(false);

        if (bannerButton != null)
            bannerButton.SetActive(false);

        if (interstitialButton != null)
            interstitialButton.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        gameEnded = true;
        Time.timeScale = 0f;
    }




    public void ShowDefeat()
    {
        if (timer != null)
            timer.StopTimer();

        if (rewardedButton != null)
            rewardedButton.SetActive(false);

        if (bannerButton != null)
            bannerButton.SetActive(false);

        if (interstitialButton != null)
            interstitialButton.SetActive(false);

        if (defeatPanel != null)
            defeatPanel.SetActive(true);

        gameEnded = true;
        Time.timeScale = 0f;
    }


}
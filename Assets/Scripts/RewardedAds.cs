using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    string _adUnitId;

    [SerializeField] Button _rewardedAdButton;
    public FlyingObjectManager flyingObjectManager;

    private void Awake()
    {
        _adUnitId = _androidAdUnitId;

        if (flyingObjectManager == null)
        {
            flyingObjectManager = FindFirstObjectByType<FlyingObjectManager>();
        }
    }

    public void LoadAd()
    {
        if(!Advertisement.isInitialized)
        {
            Debug.LogWarning("Tried to load rewarded ad before Unity ads was initialized!");
            return;
        }
        Debug.Log("Loading rewarded ad");
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Rewarded ad loaded!");

        if(placementId.Equals(_adUnitId))
        {
            _rewardedAdButton.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning("Failed to load rewarded ad!");
        StartCoroutine(WaitAndLoad(5f));
    }

    public IEnumerator WaitAndLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogWarning("Failed to show rewarded ad!");
        StartCoroutine(WaitAndLoad(5f));
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Time.timeScale = 0f;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("User clicked on rewarded ad");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Rewarded ad completed!");
            string scene = SceneManager.GetActiveScene().name;
            if (scene == "CityScene")
            {
                flyingObjectManager.DestroyAllFlyingObjects();
            }
            else if (scene == "HanojasTornis")
            {
                Timer timer = FindFirstObjectByType<Timer>();
                if (timer != null)
                {
                    timer.SubtractTime(10f);
                    Debug.Log("Reward applied: -10 seconds from timer");
                }
                else
                {
                    Debug.LogWarning("Timer component not found on scene!");
                }

                if (WinManager.Instance != null)
                {
                    WinManager.Instance.timer -= 10f;
                    if (WinManager.Instance.timer < 0)
                        WinManager.Instance.timer = 0;
                }
            }
            _rewardedAdButton.interactable = false;
            StartCoroutine(WaitAndLoad(10f));
        }
        Time.timeScale = 1f;
    }

    private void UpdateTimerUI()
    {
        if (WinManager.Instance == null) return;

        // Ищем объект по имени "Timer"
        GameObject timerObject = GameObject.Find("Timer");

        if (timerObject == null)
        {
            Debug.LogWarning("Timer object not found by name 'Timer'");
            return;
        }

        int minutes = Mathf.FloorToInt(WinManager.Instance.timer / 60);
        int seconds = Mathf.FloorToInt(WinManager.Instance.timer % 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Пробуем найти обычный Text компонент
        Text timerText = timerObject.GetComponent<Text>();
        if (timerText != null)
        {
            timerText.text = timeString;
            Debug.Log($"Timer UI updated (Text): {timeString}");
            return;
        }

        // Пробуем найти TextMeshProUGUI компонент
        TMPro.TextMeshProUGUI timerTextTMP = timerObject.GetComponent<TMPro.TextMeshProUGUI>();
        if (timerTextTMP != null)
        {
            timerTextTMP.text = timeString;
            Debug.Log($"Timer UI updated (TMP): {timeString}");
            return;
        }

        Debug.LogWarning("Timer object found but has no Text or TextMeshProUGUI component!");
    }

    public void SetButton(Button button)
    {
        if (button == null)
        {
            Debug.LogWarning("RewardedAd button not found in this scene.");
            return;
        }

        _rewardedAdButton = button;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowAd);
        _rewardedAdButton.interactable = false;

        if (Advertisement.isInitialized)
            Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        _rewardedAdButton.interactable = false;
        Advertisement.Show(_adUnitId, this);
    }
}

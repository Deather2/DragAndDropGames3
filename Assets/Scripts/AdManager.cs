using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public AdsInitializer adsInitializer;
    public InterstitialAds interstitialAd;
    [SerializeField] bool turnOffInterstitialAd = false;

    private bool isFirstLaunch = true; 
    private string previousSceneName = "";

    public static AdManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (adsInitializer == null)
        {
            adsInitializer = GetComponent<AdsInitializer>();
        }

        if (interstitialAd == null)
        {
            interstitialAd = GetComponent<InterstitialAds>();
        }

        adsInitializer.OnAdsInitialized += HandleAdsInitialized;
    }

    private void HandleAdsInitialized()
    {
        if (!turnOffInterstitialAd)
        {
            interstitialAd.LoadAd();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isFirstLaunch)
        {
            isFirstLaunch = false;
            previousSceneName = scene.name;
            Debug.Log("First launch, no ad shown");
            return;
        }

        if (previousSceneName != scene.name)
        {
            Debug.Log($"Scene changed from {previousSceneName} to {scene.name}, showing ad");

            if (interstitialAd != null && interstitialAd.isReady)
            {
                interstitialAd.ShowAd();
            }
            else
            {
                Debug.Log("Ad not ready, loading...");
                HandleAdsInitialized();
            }
        }

        previousSceneName = scene.name;

        SetupAdButtonIfExists();
    }

    private void SetupAdButtonIfExists()
    {
        GameObject buttonObj = GameObject.FindGameObjectWithTag("InterstitialAdButton");
        if (buttonObj != null)
        {
            Button interstitialButton = buttonObj.GetComponent<Button>();
            if (interstitialButton != null && interstitialAd != null)
            {
                interstitialAd.SetButton(interstitialButton);
                Debug.Log("Ad button found and configured");
            }
        }
    }
}
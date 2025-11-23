using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BannerAdsScript : MonoBehaviour
{
    [SerializeField] string _androidAdUnitId = "Banner_Android";
    string _adUnitId;

    [SerializeField] Button _bannerButton;
    public bool isBannerVisible = false;

    private void Awake()
    {
        _adUnitId = _androidAdUnitId;
    }

    void SetBannerPositionForScene()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (scene == "CityScene")
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

        else if (scene == "HanojasTornis") 
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);

        else
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    public void LoadBanner()
    {
        if (!Advertisement.isInitialized)
            return;

        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = () =>
            {
                if (_bannerButton != null)
                    _bannerButton.interactable = true;
            }
        };

        Advertisement.Banner.Load(_adUnitId, options);
    }

    public void ShowBannerAd()
    {
        if (isBannerVisible)
        {
            Advertisement.Banner.Hide(false);
            isBannerVisible = false;
            return;
        }

        Advertisement.Banner.Hide(true); 

        SetBannerPositionForScene(); 

        BannerLoadOptions loadOpts = new BannerLoadOptions
        {
            loadCallback = () =>
            {
                Advertisement.Banner.Show(_adUnitId, new BannerOptions());
                isBannerVisible = true;
            }
        };

        Advertisement.Banner.Load(_adUnitId, loadOpts);
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide(false);
        isBannerVisible = false;
    }

    public void SetButton(Button button)
    {
        if (button == null)
            return;

        _bannerButton = button;
        _bannerButton.onClick.RemoveAllListeners();
        _bannerButton.onClick.AddListener(ShowBannerAd);
        _bannerButton.interactable = false;
    }
}

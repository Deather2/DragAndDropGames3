using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class BannerAdsScript : MonoBehaviour
{
    [SerializeField] string _androidAdUnitId = "Banner_Android";
    string _adUnitId;

    public bool isBannerVisible = false;

    private void Awake()
    {
        _adUnitId = _androidAdUnitId;
    }

    public void SetBannerPositionForScene()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (scene == "MainMenu")
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

        else
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    public void ShowBanner()
    {
        Advertisement.Banner.Hide(true);

        SetBannerPositionForScene();

        BannerLoadOptions loadOpts = new BannerLoadOptions
        {
            loadCallback = () =>
            {
                Advertisement.Banner.Show(_adUnitId);
                isBannerVisible = true;
            }
        };

        Advertisement.Banner.Load(_adUnitId, loadOpts);
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide(false);
        isBannerVisible = false;
    }
}

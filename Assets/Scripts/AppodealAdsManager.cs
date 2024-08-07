using System.Collections.Generic;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;

public class AppodealAdsManager : MonoBehaviour, IBannerAdListener, IRewardedVideoAdListener, IAppodealInitializationListener
{
    [SerializeField] private string appKey = "3a98a8e06267b07d511879a6cbe338351e2017ee89712279";
    [SerializeField] private float interstitialAdInterval = 300f; // �������� � �������� (300 ������ = 5 �����)

    private bool shouldContinueGame;
    private float interstitialTimer;

    private void Start()
    {
        InitializeAppodeal();
        ShowBanner();
        interstitialTimer = interstitialAdInterval;

        if (PlayerPrefs.GetInt("noAdsPurchased", 0) == 1)
        {
            interstitialAdInterval = 0;
        }
    }

    private void Update()
    {
        // ��������� ������ ��� ���������� �������
        if (interstitialTimer > 0)
        {
            interstitialTimer -= Time.deltaTime;
            if (interstitialTimer <= 0)
            {
                ShowInterstitialAd();
                interstitialTimer = interstitialAdInterval; // ����� �������
            }
        }
    }

    private void InitializeAppodeal()
    {
        int adTypes = Appodeal.BANNER | Appodeal.REWARDED_VIDEO | Appodeal.INTERSTITIAL;
        Appodeal.initialize(appKey, adTypes, this);

        Appodeal.setBannerCallbacks(this);
        Appodeal.setRewardedVideoCallbacks(this);
    }

    public void onInitializationFinished(List<string> errors)
    {
        throw new System.NotImplementedException();
    }

    private void ShowInterstitialAd()
    {
        if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
        {
            Appodeal.show(Appodeal.INTERSTITIAL);
        }
        else
        {
            Appodeal.cache(Appodeal.INTERSTITIAL);
        }
    }

    public void ShowBanner()
    {
        Appodeal.show(Appodeal.BANNER_BOTTOM, "default");
    }

    public void HideBanner()
    {
        Appodeal.hide(Appodeal.BANNER);
    }

    public void ShowRewardedVideo(bool continueGame = false)
    {
        shouldContinueGame = continueGame;
        if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO) && Appodeal.canShow(Appodeal.REWARDED_VIDEO, "default"))
        {
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        }
        else
        {
            Appodeal.cache(Appodeal.REWARDED_VIDEO);
        }
    }

    // IBannerAdListener implementation
    public void onBannerLoaded(int height, bool isPrecache) { }
    public void onBannerFailedToLoad() { }
    public void onBannerShown() { }
    public void onBannerShowFailed() { }
    public void onBannerClicked() { }
    public void onBannerExpired() { }

    // IRewardedVideoAdListener implementation
    public void onRewardedVideoLoaded(bool isPrecache) { }
    public void onRewardedVideoFailedToLoad() { }
    public void onRewardedVideoShown() { }
    public void onRewardedVideoShowFailed() { }
    public void onRewardedVideoClosed(bool finished)
    {
        if (finished && shouldContinueGame)
        {
            GameManager.Instance.ContinueGame();
        }
        shouldContinueGame = false;
    }
    public void onRewardedVideoFinished(double amount, string name) { }
    public void onRewardedVideoExpired() { }
    public void onRewardedVideoClicked() { }
}

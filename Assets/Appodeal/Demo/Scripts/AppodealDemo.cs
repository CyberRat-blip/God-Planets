using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;
using UnityEngine.UI;

namespace AppodealAds.Demo.Scripts
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    public class AppodealDemo : MonoBehaviour, IAppodealInitializationListener, IInAppPurchaseValidationListener,
                                IBannerAdListener, IInterstitialAdListener, IRewardedVideoAdListener, IMrecAdListener,
                                IAdRevenueListener
    {
        #region Constants

        private const string CACHE_INTERSTITIAL = "CACHE INTERSTITIAL";
        private const string SHOW_INTERSTITIAL = "SHOW INTERSTITIAL";
        private const string CACHE_REWARDED_VIDEO = "CACHE REWARDED VIDEO";

        #endregion

        #region UI

        [SerializeField] public Toggle tgTesting;
        [SerializeField] public Toggle tgLogging;
        [SerializeField] public Button btnShowInterstitial;
        [SerializeField] public Button btnShowRewardedVideo;
        [SerializeField] public GameObject appodealPanel;

        #endregion

        #region Application keys

#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IPHONE
        public static string appKey = "";
#elif UNITY_ANDROID
        public static string appKey = "fee50c333ff3825fd6ad6d38cff78154de3025546d47a84f";
#elif UNITY_IPHONE
        public static string appKey = "466de0d625e01e8811c588588a42a55970bc7c132649eede";
#else
	    public static string appKey = "";
#endif

        #endregion

        private void Start()
        {
            appodealPanel.gameObject.SetActive(true);

            btnShowInterstitial.GetComponentInChildren<Text>().text = CACHE_INTERSTITIAL;
            btnShowRewardedVideo.GetComponentInChildren<Text>().text = CACHE_REWARDED_VIDEO;
        }

        private void OnDestroy()
        {
            Appodeal.destroy(Appodeal.BANNER);
        }

        public void Initialize()
        {
            InitAppodeal();
        }

        public void InitAppodeal()
        {
            Appodeal.setLogLevel(tgLogging.isOn ? Appodeal.LogLevel.Verbose : Appodeal.LogLevel.None);
            Appodeal.setTesting(tgTesting.isOn);
            Appodeal.setUseSafeArea(true);

            Appodeal.setUserId("1");
            Appodeal.setCustomFilter(UserSettings.USER_AGE, 18);
            Appodeal.setCustomFilter(UserSettings.USER_GENDER, (int) UserSettings.Gender.MALE);

            Appodeal.setExtraData("testKey", "testValue");
            Appodeal.resetExtraData("testKey");

            Appodeal.setSmartBanners(true);
            Appodeal.setBannerAnimation(false);
            Appodeal.setTabletBanners(false);
            Appodeal.setBannerRotation(-90, 110);

            Appodeal.disableLocationPermissionCheck();
            Appodeal.setChildDirectedTreatment(false);
            Appodeal.muteVideosIfCallsMuted(true);

            Appodeal.setTriggerOnLoadedOnPrecache(Appodeal.INTERSTITIAL, true);

            Appodeal.disableNetwork(AppodealNetworks.VUNGLE);
            Appodeal.disableNetwork(AppodealNetworks.YANDEX, Appodeal.MREC);

            Appodeal.setAutoCache(Appodeal.INTERSTITIAL, false);
            Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO, false);

            Appodeal.setBannerCallbacks(this);
            Appodeal.setInterstitialCallbacks(this);
            Appodeal.setRewardedVideoCallbacks(this);
            Appodeal.setMrecCallbacks(this);
            Appodeal.setAdRevenueCallback(this);

            Appodeal.setCustomFilter("newBoolean", true);
            Appodeal.setCustomFilter("newInt", 1234567890);
            Appodeal.setCustomFilter("newDouble", 123.123456789);
            Appodeal.setCustomFilter("newString", "newStringFromSDK");

            int adTypes = Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.REWARDED_VIDEO | Appodeal.MREC;
            Appodeal.initialize(appKey, adTypes, this);
        }

        public void ShowInterstitial()
        {
            if (Appodeal.isLoaded(Appodeal.INTERSTITIAL) && Appodeal.canShow(Appodeal.INTERSTITIAL, "default") && !Appodeal.isPrecache(Appodeal.INTERSTITIAL))
            {
                Appodeal.show(Appodeal.INTERSTITIAL);
            }
            else
            {
                Appodeal.cache(Appodeal.INTERSTITIAL);
            }
        }

        public void ShowRewardedVideo()
        {
            if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO) && Appodeal.canShow(Appodeal.REWARDED_VIDEO, "default"))
            {
                Appodeal.show(Appodeal.REWARDED_VIDEO);
            }
            else
            {
                Appodeal.cache(Appodeal.REWARDED_VIDEO);
            }
        }

        public void ShowBannerBottom()
        {
            Appodeal.show(Appodeal.BANNER_BOTTOM, "default");
        }

        public void ShowBannerTop()
        {
            Appodeal.show(Appodeal.BANNER_TOP, "default");
        }

        public void HideBanner()
        {
            Appodeal.hide(Appodeal.BANNER);
        }

        public void ShowBannerView()
        {
            Appodeal.showBannerView(Appodeal.BANNER_BOTTOM, Appodeal.BANNER_HORIZONTAL_CENTER, "default");
        }

        public void HideBannerView()
        {
            Appodeal.hideBannerView();
        }

        public void ShowMrecView()
        {
            Appodeal.showMrecView(Appodeal.BANNER_TOP, Appodeal.BANNER_HORIZONTAL_CENTER, "default");
        }

        public void HideMrecView()
        {
            Appodeal.hideMrecView();
        }

        public void ShowBannerLeft()
        {
            Appodeal.show(Appodeal.BANNER_LEFT);
        }

        public void ShowBannerRight()
        {
            Appodeal.show(Appodeal.BANNER_RIGHT);
        }

        #region AppodealInitializeListener

        public void onInitializationFinished(List<string> errors)
        {
            string output = errors == null ? string.Empty : string.Join(", ", errors);
            Debug.Log($"onInitializationFinished(errors:[{output}])");

            Debug.Log($"isAutoCacheEnabled() for banner: {Appodeal.isAutoCacheEnabled(Appodeal.BANNER)}");
            Debug.Log($"isInitialized() for banner: {Appodeal.isInitialized(Appodeal.BANNER)}");
            Debug.Log($"isSmartBannersEnabled(): {Appodeal.isSmartBannersEnabled()}");
            Debug.Log($"getUserId(): {Appodeal.getUserId()}");
            Debug.Log($"getSegmentId(): {Appodeal.getSegmentId()}");
            Debug.Log($"getRewardParameters(): {Appodeal.getRewardParameters()}");
            Debug.Log($"getNativeSDKVersion(): {Appodeal.getNativeSDKVersion()}");

            var networksList = Appodeal.getNetworks(Appodeal.REWARDED_VIDEO);
            output = networksList == null ? string.Empty : string.Join(", ", (networksList.ToArray()));
            Debug.Log($"getNetworks() for RV: {output}");

#if UNITY_ANDROID
            var additionalParams = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };

            var purchase = new PlayStoreInAppPurchase.Builder(Appodeal.PlayStorePurchaseType.Subs)
                .withAdditionalParameters(additionalParams)
                .withPurchaseTimestamp(793668600)
                .withDeveloperPayload("payload")
                .withPurchaseToken("token")
                .withPurchaseData("data")
                .withPublicKey("key")
                .withSignature("signature")
                .withCurrency("USD")
                .withOrderId("orderId")
                .withPrice("1.99")
                .withSku("sku")
                .build();

            Appodeal.validatePlayStoreInAppPurchase(purchase, this);
#elif UNITY_IOS
            var additionalParams = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };

            var purchase = new AppStoreInAppPurchase.Builder(Appodeal.AppStorePurchaseType.Consumable)
                .withAdditionalParameters(additionalParams)
                .withTransactionId("transactionId")
                .withProductId("productId")
                .withCurrency("USD")
                .withPrice("2.89")
                .build();

            Appodeal.validateAppStoreInAppPurchase(purchase, this);
#endif

            Appodeal.logEvent("test_event", new Dictionary<string, object> { { "test_key_1", 42 }, { "test_key_2", "test_value" } });
        }

        #endregion

        #region InAppPurchaseValidationListener

        public void onInAppPurchaseValidationSucceeded(string json)
        {
            Debug.Log($"onInAppPurchaseValidationSucceeded(string json:\n{json})");
        }

        public void onInAppPurchaseValidationFailed(string json)
        {
            Debug.Log($"onInAppPurchaseValidationFailed(string json:\n{json})");
        }

        #endregion

        #region Banner callback handlers

        public void onBannerLoaded(int height, bool precache)
        {
            Debug.Log("onBannerLoaded");
            Debug.Log($"Banner height - {height}");
            Debug.Log($"Banner precache - {precache}");
            Debug.Log($"getPredictedEcpm(): {Appodeal.getPredictedEcpm(Appodeal.BANNER)}");
        }

        public void onBannerFailedToLoad()
        {
            Debug.Log("onBannerFailedToLoad");
        }

        public void onBannerShown()
        {
            Debug.Log("onBannerShown");
        }

        public void onBannerShowFailed()
        {
            Debug.Log("onBannerShowFailed");
        }

        public void onBannerClicked()
        {
            Debug.Log("onBannerClicked");
        }

        public void onBannerExpired()
        {
            Debug.Log("onBannerExpired");
        }

        #endregion

        #region Interstitial callback handlers

        public void onInterstitialLoaded(bool isPrecache)
        {
            if (!isPrecache)
            {
                btnShowInterstitial.GetComponentInChildren<Text>().text = SHOW_INTERSTITIAL;
            }
            else
            {
                Debug.Log("Appodeal. Interstitial loaded. isPrecache - true");
            }

            Debug.Log("onInterstitialLoaded");
            Debug.Log($"getPredictedEcpm(): {Appodeal.getPredictedEcpm(Appodeal.INTERSTITIAL)}");
        }

        public void onInterstitialFailedToLoad()
        {
            Debug.Log("onInterstitialFailedToLoad");
        }

        public void onInterstitialShowFailed()
        {
            Debug.Log("onInterstitialShowFailed");
        }

        public void onInterstitialShown()
        {
            Debug.Log("onInterstitialShown");
        }

        public void onInterstitialClosed()
        {
            btnShowInterstitial.GetComponentInChildren<Text>().text = CACHE_INTERSTITIAL;
            Debug.Log("onInterstitialClosed");
        }

        public void onInterstitialClicked()
        {
            Debug.Log("onInterstitialClicked");
        }

        public void onInterstitialExpired()
        {
            Debug.Log("onInterstitialExpired");
        }

        #endregion

        #region Rewarded Video callback handlers

        public void onRewardedVideoLoaded(bool isPrecache)
        {
            btnShowRewardedVideo.GetComponentInChildren<Text>().text = "SHOW REWARDED VIDEO";
            Debug.Log("onRewardedVideoLoaded");
            Debug.Log($"getPredictedEcpm(): {Appodeal.getPredictedEcpm(Appodeal.REWARDED_VIDEO)}");
        }

        public void onRewardedVideoFailedToLoad()
        {
            Debug.Log("onRewardedVideoFailedToLoad");
        }

        public void onRewardedVideoShowFailed()
        {
            Debug.Log("onRewardedVideoShowFailed");
        }

        public void onRewardedVideoShown()
        {
            Debug.Log("onRewardedVideoShown");
        }

        public void onRewardedVideoClosed(bool finished)
        {
            btnShowRewardedVideo.GetComponentInChildren<Text>().text = "CACHE REWARDED VIDEO";
            Debug.Log($"onRewardedVideoClosed. Finished - {finished}");
        }

        public void onRewardedVideoFinished(double amount, string name)
        {
            Debug.Log("onRewardedVideoFinished. Reward: " + amount + " " + name);
        }

        public void onRewardedVideoExpired()
        {
            Debug.Log("onRewardedVideoExpired");
        }

        public void onRewardedVideoClicked()
        {
            Debug.Log("onRewardedVideoClicked");
        }

        #endregion

        #region Mrec callback handlers

        public void onMrecLoaded(bool precache)
        {
            Debug.Log($"onMrecLoaded. Precache - {precache}");
            Debug.Log($"getPredictedEcpm(): {Appodeal.getPredictedEcpm(Appodeal.MREC)}");
        }

        public void onMrecFailedToLoad()
        {
            Debug.Log("onMrecFailedToLoad");
        }

        public void onMrecShown()
        {
            Debug.Log("onMrecShown");
        }

        public void onMrecShowFailed()
        {
            Debug.Log("onMrecShowFailed");
        }

        public void onMrecClicked()
        {
            Debug.Log("onMrecClicked");
        }

        public void onMrecExpired()
        {
            Debug.Log("onMrecExpired");
        }

        #endregion

        #region IAdRevenueListener implementation

        public void onAdRevenueReceived(AppodealAdRevenue ad)
        {
            Debug.Log($"[APDUnity] [Callback] onAdRevenueReceived({ad.ToJsonString(true)})");
        }

        #endregion
    }
}

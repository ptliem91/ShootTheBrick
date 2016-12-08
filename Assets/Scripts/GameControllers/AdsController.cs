using UnityEngine;
using System.Collections;

//using UnityEngine.Advertisements;
//using ChartboostSDK;
//using UnityEngine.SceneManagement;

public class AdsController : MonoBehaviour
{
	//
	//	public static AdsController instance;
	//
	//	//app id for unity ads
	//	private const string app_id = "1221373";
	//
	//	public bool canShowChartboostInterstitial;
	//	public bool canShowChartboostVideo;
	//
	//	void Awake ()
	//	{
	//		MakeSingleton ();
	//
	//		if (!canShowChartboostInterstitial) {
	//			LoadChartboostInterstitialAds ();
	//		}
	//
	//		if (!canShowChartboostVideo) {
	//			LoadChartboostVideoAds ();
	//		}
	//
	//		LoadUnityAds ();
	//	}
	//
	//	//	void OnLevelWasLoaded ()
	//	//	{
	//	//		if (SceneManager.GetActiveScene ().name == "GP_Lvl_Select") {
	//	//			if (canShowChartboostInterstitial) {
	//	//				ShowChartboostInterstitial ();
	//	//			} else {
	//	//				LoadChartboostInterstitialAds ();
	//	//			}
	//	//		}
	//	//	}
	//
	//	void MakeSingleton ()
	//	{
	//		if (instance != null) {
	//			Destroy (gameObject);
	//		} else {
	//			instance = this;
	//			DontDestroyOnLoad (gameObject);
	//		}
	//	}
	//
	//	void OnEnable ()
	//	{
	//		Chartboost.didCompleteRewardedVideo += VideoCompleted;
	//		Chartboost.didCacheInterstitial += DidCacheInterstitial;
	//		Chartboost.didDismissInterstitial += DidDismissInterstitial;
	//		Chartboost.didCloseInterstitial += DidCloseInterstitial;
	//		Chartboost.didCacheRewardedVideo += DidCacheRewardedVideo;
	//		Chartboost.didFailToLoadInterstitial += FailedToLoadInterstitial;
	//		Chartboost.didFailToLoadRewardedVideo += FailedToLoadRewardedVideo;
	//	}
	//
	//	void OnDisable ()
	//	{
	//		Chartboost.didCompleteRewardedVideo -= VideoCompleted;
	//		Chartboost.didCacheInterstitial -= DidCacheInterstitial;
	//		Chartboost.didDismissInterstitial -= DidDismissInterstitial;
	//		Chartboost.didCloseInterstitial -= DidCloseInterstitial;
	//		Chartboost.didCacheRewardedVideo -= DidCacheRewardedVideo;
	//		Chartboost.didFailToLoadInterstitial -= FailedToLoadInterstitial;
	//		Chartboost.didFailToLoadRewardedVideo -= FailedToLoadRewardedVideo;
	//	}
	//
	//	public void VideoCompleted (CBLocation location, int reward)
	//	{
	//		canShowChartboostVideo = false;
	//
	//		if (ShopMenuController.instance != null) {
	//			ShopMenuController.instance.GiveUserRewardVideoWatched ();
	//		}
	//
	//		LoadChartboostVideoAds ();
	//	}
	//
	//	public void DidCacheInterstitial (CBLocation location)
	//	{
	//		canShowChartboostInterstitial = true;
	//	}
	//
	//	public void DidDismissInterstitial (CBLocation location)
	//	{
	//		canShowChartboostInterstitial = false;
	//		LoadChartboostVideoAds ();
	//	}
	//
	//	public void DidCloseInterstitial (CBLocation location)
	//	{
	//		canShowChartboostInterstitial = false;
	//		LoadChartboostVideoAds ();
	//	}
	//
	//	public void DidCacheRewardedVideo (CBLocation location)
	//	{
	//		canShowChartboostVideo = true;
	//	}
	//
	//	public void FailedToLoadInterstitial (CBLocation location, CBImpressionError error)
	//	{
	//		canShowChartboostInterstitial = false;
	//		LoadChartboostInterstitialAds ();
	//	}
	//
	//	public void FailedToLoadRewardedVideo (CBLocation location, CBImpressionError error)
	//	{
	//		canShowChartboostVideo = false;
	//
	//		if (ShopMenuController.instance != null) {
	//			ShopMenuController.instance.FailedToLoadTheVideoAds ();
	//		}
	//
	//		LoadChartboostVideoAds ();
	//	}
	//
	//	//
	//	public void LoadChartboostVideoAds ()
	//	{
	//		Chartboost.cacheRewardedVideo (CBLocation.Default);
	//	}
	//
	//	public void LoadChartboostInterstitialAds ()
	//	{
	//		Chartboost.cacheInterstitial (CBLocation.Default);
	//	}
	//
	//	public void ShowChartboostInterstitial ()
	//	{
	//		if (canShowChartboostInterstitial) {
	//			Chartboost.showInterstitial (CBLocation.Default);
	//		} else {
	//			LoadChartboostInterstitialAds ();
	//		}
	//	}
	//
	//	public void ShowChartboostVideo ()
	//	{
	//		if (canShowChartboostVideo) {
	//			Chartboost.showRewardedVideo (CBLocation.Default);
	//		} else {
	//			LoadChartboostVideoAds ();
	//		}
	//	}
	//
	//	public void LoadUnityAds ()
	//	{
	//		if (Advertisement.isSupported) {
	//			Advertisement.Initialize (app_id, true);
	//		}
	//	}
	//
	//	public void ShowUnityAds ()
	//	{
	//		if (Advertisement.IsReady ()) {
	//
	//			ShowOptions options = new ShowOptions ();
	//			options.resultCallback = AdCallbackhandler;
	//
	//			Advertisement.Show (null, options);
	//
	//		} else {
	//			GameplayController.instance.VideoNotLoadedOrUserSkippedTheVideo ("Failed to load video. Please try again or check your network.");
	//			LoadUnityAds ();
	//		}
	//	}
	//
	//	void AdCallbackhandler (ShowResult result)
	//	{
	//		switch (result) {
	//		case ShowResult.Finished:
	//			GameplayController.instance.VideoWatchedGivePlayerLives ();
	//			LoadUnityAds ();
	//			break;
	//
	//		case ShowResult.Failed:
	//			GameplayController.instance.VideoNotLoadedOrUserSkippedTheVideo ("Failed to load video. Please try again or check your network.");
	//			LoadUnityAds ();
	//			break;
	//
	//		case ShowResult.Skipped:
	//			GameplayController.instance.VideoNotLoadedOrUserSkippedTheVideo ("Video skipped. No lives earned.");
	//			LoadUnityAds ();
	//			break;
	//		}
	//	}
}

using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class UnityAdsController : MonoBehaviour
{
	public static UnityAdsController instance;

	private const string app_id = "1221718";

	void Awake ()
	{
		MakeSingleton ();
	
		LoadUnityAds ();
	}

	void MakeSingleton ()
	{
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	public void LoadUnityAds ()
	{
		if (Advertisement.isSupported) {
//			Advertisement.allowPrecache = true;
			Advertisement.Initialize (app_id, true);
		}
	}

	public void ShowUnityAdsRewardedGiveCoins ()
	{
		if (Advertisement.IsReady ("rewardedVideo")) {
			
			var options = new ShowOptions { resultCallback = HandleShowResultGiveCoins };

			Advertisement.Show ("rewardedVideo", options);
		}
	}

	public void ShowUnityAdsGiveLives ()
	{
		if (Advertisement.IsReady ()) {
	
			Advertisement.Show (null, new ShowOptions () {
				resultCallback = HandleShowResultGiveLives
			});
	
		} else {
			GameplayController.instance.VideoNotLoadedOrUserSkippedTheVideo ("Failed to load video. Please try again or check your network.");
			LoadUnityAds ();
		}
	}

	private void HandleShowResultGiveLives (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			GameplayController.instance.VideoWatchedGivePlayerLives ();
			LoadUnityAds ();
			break;
	
		case ShowResult.Failed:
			GameplayController.instance.VideoNotLoadedOrUserSkippedTheVideo ("Failed to load video. Please try again or check your network.");
			LoadUnityAds ();
			break;
	
		case ShowResult.Skipped:
			GameplayController.instance.VideoNotLoadedOrUserSkippedTheVideo ("Video skipped. No lives earned.");
			LoadUnityAds ();
			break;
		}
	}

	private void HandleShowResultGiveCoins (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			ShopMenuController.instance.GiveUserRewardVideoWatched ();
			LoadUnityAds ();
			break;

		case ShowResult.Skipped:
			ShopMenuController.instance.FailedToLoadTheVideoAds ();
			LoadUnityAds ();
			break;

		case ShowResult.Failed:
			ShopMenuController.instance.FailedToLoadTheVideoAds ();
			LoadUnityAds ();
			break;
		}
	}
}

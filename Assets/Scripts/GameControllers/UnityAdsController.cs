using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System;

public class UnityAdsController : MonoBehaviour
{
	public static UnityAdsController instance;

	private const string APP_ID = "1221718";
	private const bool TEST_MODE = true;

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
		try {
			if (Advertisement.isSupported) {
//			Advertisement.allowPrecache = true;
				Advertisement.Initialize (APP_ID, TEST_MODE);
			}

		} catch (Exception ex) {
			print (ex.Message);
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
			GameplayController.instance.VideoNotLoadedOrUserSkippedTheVideo ("Failed to load video. Please try again or check your network connection.");
			LoadUnityAds ();
		}
	}

	private void HandleShowResultGiveLives (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:
			GameplayController.instance.VideoWatchedGivePlayerLives (true);
			LoadUnityAds ();
			break;
	
		case ShowResult.Failed:
			GameplayController.instance.VideoNotLoadedOrUserSkippedTheVideo ("Failed to load video. Please try again or check your network connection.");
			LoadUnityAds ();
			break;
	
		case ShowResult.Skipped:
//			GameplayController.instance.VideoNotLoadedOrUserSkippedTheVideo ("Video skipped.");
			GameplayController.instance.VideoWatchedGivePlayerLives (true);
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
			ShopMenuController.instance.FailedToLoadTheVideoAds ("Video skipped. No earn coins.");
			LoadUnityAds ();
			break;

		case ShowResult.Failed:
			ShopMenuController.instance.FailedToLoadTheVideoAds ("Failed to load video. Please try again or check your network connection.");
			LoadUnityAds ();
			break;
		}
	}
}

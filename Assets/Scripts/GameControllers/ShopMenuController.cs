using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopMenuController : MonoBehaviour
{
	public static ShopMenuController instance;

	public Text coinText, buyArrowsText, watchVideoText, messageText;
	//scoreText,

	public Button weaponsTabBtn, earnCoinTabBtn, yesBtn;

	public GameObject weaponItemsPanel, earnCoinsItemsPanel, buyArrowsPanel;

	private const int priceDoubleSwords = 4000;
	private const int priceTripleSwords = 7000;
	private const int priceHarpoonChain = 9000;

	void Awake ()
	{
		MakeInstance ();
	}

	// Use this for initialization
	void Start ()
	{
		InitializeShopMenuController ();
	}

	void MakeInstance ()
	{
		if (instance == null) {
			instance = this;
		}
	}

	void InitializeShopMenuController ()
	{
		coinText.text = "" + GameController.instance.coins;
//		scoreText.text = "" + GameController.instance.highScore;
	}

	public void BuyDoubleArrows ()
	{
		if (!GameController.instance.weapons [1]) {
			if (GameController.instance.coins >= priceDoubleSwords) {
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "Do you want to purchase Double Swords ?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyArrow (1));

			} else {
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "You don't have enough Coins. Do you want to earn Coins ?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenEarnCoinsItemsPanel ());
			}
		}
	}

	public void BuyStickyArrows ()
	{
		if (!GameController.instance.weapons [2]) {
			if (GameController.instance.coins >= priceTripleSwords) {
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "Do you want to purchase Triple Swords ?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyArrow (2));

			} else {
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "You don't have enough Coins. Do you want to earn Coins ?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenEarnCoinsItemsPanel ());
			}
		}
	}

	public void BuyDoubleStickyArrows ()
	{
		if (!GameController.instance.weapons [3]) {
			if (GameController.instance.coins >= priceHarpoonChain) {
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "Do you want to purchase Harpoon Chain ?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyArrow (3));

			} else {
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "You don't have enough Coins. Do you want to earn Coins ?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenEarnCoinsItemsPanel ());
			}
		}
	}

	public void BuyArrow (int index)
	{
		int payCoins = priceDoubleSwords;

		if (index == 2) {
			payCoins = priceTripleSwords;

		} else if (index == 3) {
			payCoins = priceHarpoonChain;
		}
		
		GameController.instance.weapons [index] = true;
		GameController.instance.coins -= payCoins;
		GameController.instance.Save ();

		buyArrowsPanel.SetActive (false);
		coinText.text = "" + GameController.instance.coins;
	}

	//	public void OpenCoinShop ()
	//	{
	//		if (buyArrowsPanel.activeInHierarchy) {
	//			buyArrowsPanel.SetActive (false);
	//		}
	//
	//		coinShopPanel.SetActive (true);
	//	}

	//	public void CloseCoinShop ()
	//	{
	//		coinShopPanel.SetActive (false);
	//	}

	public void OpenWeaponItemsPanel ()
	{
		weaponItemsPanel.SetActive (true);
//		specialItemsPanel.SetActive (false);
		earnCoinsItemsPanel.SetActive (false);
	}

	public void OpenSpecialItemsPanel ()
	{
		weaponItemsPanel.SetActive (false);
//		specialItemsPanel.SetActive (true);
		earnCoinsItemsPanel.SetActive (false);
	}

	public void OpenEarnCoinsItemsPanel ()
	{
		weaponItemsPanel.SetActive (false);
//		specialItemsPanel.SetActive (false);
		earnCoinsItemsPanel.SetActive (true);
		messageText.text = "";

		buyArrowsPanel.SetActive (false);
	}

	public void GoToLevelMenu ()
	{
		SceneManager.LoadScene ("GP_Lvl_Select");
	}

	public void GoToMainMenu ()
	{
		SceneManager.LoadScene ("GP_Main_Menu");
	}

	public void DontBuyArrows ()
	{
		buyArrowsPanel.SetActive (false);
	}

	//Ads
	public void WatchVideoEarnCoins ()
	{
		int day = DateTime.Now.Day - GameController.instance.dateTimeForWatchVideoAds.Day;

		if (day >= 1) {
			//watch
			UnityAdsController.instance.ShowUnityAdsRewardedGiveCoins ();

		} else {
			TimeSpan time = DateTime.Now.TimeOfDay - GameController.instance.dateTimeForWatchVideoAds.TimeOfDay;

			int waitTime = 10 - time.Minutes; //10 is wait time to watch video next

			if (waitTime <= 0) {
				UnityAdsController.instance.ShowUnityAdsRewardedGiveCoins ();

			} else {
				messageText.text = "You need to wait " + waitTime + " minutes to watch video.";
			}
		}

//		UnityAdsController.instance.ShowUnityAdsRewardedGiveCoins ();
	}

	public void GiveUserRewardVideoWatched ()
	{
		GameController.instance.coins += 300;
		GameController.instance.dateTimeForWatchVideoAds = DateTime.Now;

		GameController.instance.Save ();

		coinText.text = "" + GameController.instance.coins;
		messageText.text = "You've received 300 coins :)";
	}

	public void FailedToLoadTheVideoAds (String message)
	{
//		watchVideoText.text = "Could not load video.";
		messageText.text = message;
	}

	//Facebook share
	public void ShareOnFacebookButton ()
	{
		bool hasInternet = InternetChecker.instance.isConnected;

		if (hasInternet) {
			int day = DateTime.Now.Day - GameController.instance.dateTimeForPostingOnFacebook.Day;

			if (day >= 2) {
				//share
				FacebookController.instance.Share ();
			} else if (day == 1) {
				if (DateTime.Now.Hour >= GameController.instance.dateTimeForPostingOnFacebook.Hour) {
					//share
					FacebookController.instance.Share ();
				} else {
					int waitTime = GameController.instance.dateTimeForPostingOnFacebook.Hour - DateTime.Now.Hour;

					messageText.text = "You need to wait " + waitTime + " hour(s) to post.";
				}
			} else {
				TimeSpan time = DateTime.Now.TimeOfDay - GameController.instance.dateTimeForPostingOnFacebook.TimeOfDay;
				int waitTime = 24 - time.Hours;

				messageText.text = "You need to wait " + waitTime + " hour(s) to post.";
			}

		} else {
			messageText.text = "Please try again or check your network connection.";
		}

		//Test
//		FacebookController.instance.Share ();
	}

	public void PostFacebookSucceded ()
	{
		GameController.instance.coins += 400;
		GameController.instance.dateTimeForPostingOnFacebook = DateTime.Now;
		GameController.instance.Save ();

		coinText.text = "" + GameController.instance.coins;
		messageText.text = "Thank you for Sharing on Facebook. You've received 400 coins :)";
	}

	public void PostFacebookFailed ()
	{
		messageText.text = "Share failed. Please login facebook or check your network connection.";
	}
}









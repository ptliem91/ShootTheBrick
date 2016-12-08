using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopMenuController : MonoBehaviour
{
	public static ShopMenuController instance;

	public Text coinText, scoreText, buyArrowsText, watchVideoText;

	public Button weaponsTabBtn, earnCoinTabBtn, yesBtn;

	public GameObject weaponItemsPanel, earnCoinsItemsPanel, buyArrowsPanel;

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
		scoreText.text = "" + GameController.instance.highScore;
	}

	public void BuyDoubleArrows ()
	{
		if (!GameController.instance.weapons [1]) {
			if (GameController.instance.coins >= 7000) {
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "Do you want to purchase Double Arrows ?";
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
			if (GameController.instance.coins >= 7000) {
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "Do you want to purchase Double Arrows ?";
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
			if (GameController.instance.coins >= 7000) {
				buyArrowsPanel.SetActive (true);
				buyArrowsText.text = "Do you want to purchase Double Arrows ?";
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
		GameController.instance.weapons [index] = true;
		GameController.instance.coins -= 7000;
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
//		AdsController.instance.ShowChartboostVideo ();
	}

	public void GiveUserRewardVideoWatched ()
	{
		GameController.instance.coins += 300;
		GameController.instance.Save ();

		coinText.text = "" + GameController.instance.coins;
	}

	public void FailedToLoadTheVideoAds ()
	{
		watchVideoText.text = "Could not load video.";
	}
}









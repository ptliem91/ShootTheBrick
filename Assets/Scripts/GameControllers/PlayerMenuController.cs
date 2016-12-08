using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMenuController : MonoBehaviour
{
	public Text scoreText, coinText;

	public bool[] players;

	public bool[] weapons;

	public Image[] priceTags;

	public Image[] weaponIcons;

	public Sprite[] weaponArrows;

	public int selectedWeapon;

	public int selectedPlayer;

	//Buy panel
	public GameObject buyPlayerPanel;
	public Button yesBtn;
	public Text buyPlayertext;

	public GameObject coinShop;

 
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void InitializePlayerMenuController ()
	{
		scoreText.text = "" + GameController.instance.highScore;
		coinText.text = "" + GameController.instance.coins;

		players = GameController.instance.players;
		weapons = GameController.instance.weapons;
		selectedPlayer = GameController.instance.selectedPlayer;
		selectedWeapon = GameController.instance.selectedWeapon;

		for (int i = 0; i < weaponIcons.Length; i++) {
			weaponIcons [i].gameObject.SetActive (false);
		}

		for (int i = 1; i < players.Length; i++) {
			if (players [i]) {
				priceTags [i - 1].gameObject.SetActive (false);
			}
		}

		weaponIcons [selectedPlayer].gameObject.SetActive (true);
		weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
	}

	public void Player1Button ()
	{
		if (selectedPlayer != 0) {
			selectedPlayer = 0;
			selectedWeapon = 0;

			weaponIcons [selectedPlayer].gameObject.SetActive (true);
			weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];

			for (int i = 0; i < weaponIcons.Length; i++) {
				if (i == selectedPlayer) {
					continue;
				}
				weaponIcons [i].gameObject.SetActive (false);
			}

			GameController.instance.selectedPlayer = selectedPlayer;
			GameController.instance.selectedWeapon = selectedWeapon;
			GameController.instance.Save ();
		} else {
			selectedWeapon++;

			if (selectedWeapon == weapons.Length) {
				selectedWeapon = 0;
			}

			bool foundWeapon = true;

			while (foundWeapon) {
				if (weapons [selectedWeapon]) {
					weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
					GameController.instance.selectedWeapon = selectedWeapon;
					GameController.instance.Save ();
					foundWeapon = false;
				} else {
					selectedWeapon++;

					if (selectedWeapon == weapons.Length) {
						selectedWeapon = 0;
					} 
				}
			}
		}
	}
	//Player 1 Button

	public void Player2Button ()
	{
		int index = 1;
		if (players [index]) {

			if (selectedPlayer != index) {
				selectedPlayer = index;
				selectedWeapon = 0;

				weaponIcons [selectedPlayer].gameObject.SetActive (true);
				weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];

				for (int i = 0; i < weaponIcons.Length; i++) {
					if (i == selectedPlayer) {
						continue;
					}
					weaponIcons [i].gameObject.SetActive (false);
				}

				GameController.instance.selectedPlayer = selectedPlayer;
				GameController.instance.selectedWeapon = selectedWeapon;
				GameController.instance.Save ();
			} else {
				selectedWeapon++;

				if (selectedWeapon == weapons.Length) {
					selectedWeapon = 0;
				}

				bool foundWeapon = true;

				while (foundWeapon) {
					if (weapons [selectedWeapon]) {
						weaponIcons [selectedPlayer].sprite = weaponArrows [selectedWeapon];
						GameController.instance.selectedWeapon = selectedWeapon;
						GameController.instance.Save ();
						foundWeapon = false;
					} else {
						selectedWeapon++;

						if (selectedWeapon == weapons.Length) {
							selectedWeapon = 0;
						} 
					}
				}
			}

		} else {

			if (GameController.instance.coins >= 7000) {
				buyPlayerPanel.SetActive (true);
				buyPlayertext.text = "Do you want to purchase the Player ?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => BuyPlayer (index));
			} else {
				buyPlayerPanel.SetActive (true);
				buyPlayertext.text = "You don't have enough coins. Do you want to purchase Coins ?";
				yesBtn.onClick.RemoveAllListeners ();
				yesBtn.onClick.AddListener (() => OpenCoinShop ());
			}
		}
	}
	//Player 2 Button

	public void BuyPlayer (int index)
	{
		GameController.instance.players [index] = true;
		GameController.instance.coins -= 7000;
		GameController.instance.Save ();

		InitializePlayerMenuController ();
	}

	public void OpenCoinShop ()
	{
		if (buyPlayerPanel.activeInHierarchy) {
			buyPlayerPanel.SetActive (false);
		}

		coinShop.SetActive (true);
	}

	public void CloseCoinShop ()
	{
		coinShop.SetActive (false);
	}

	public void DontBuyPlayerOrCoins ()
	{
		buyPlayerPanel.SetActive (false);
	}

	public void GoToLevelMenu ()
	{
		SceneManager.LoadScene ("GP_Lvl_Select");
	}

	public void GoToMainMenu ()
	{
		SceneManager.LoadScene ("GP_Main_Menu");
	}
}

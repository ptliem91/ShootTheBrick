using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
	public static GamePlayController instance;

	[SerializeField]
	private GameObject panelSuccess;

	[SerializeField]
	private GameObject inactivePlayer;

	[SerializeField]
	private Text txtLevelSuccess;

	//
	bool isPaused = false;

	// Use this for initialization
	void Awake ()
	{
		MakeInstance ();
	}

	void MakeInstance ()
	{
		if (instance == null) {
			instance = this;
		}
	}

	void Start ()
	{
		Time.timeScale = 1f;
	}

	void Update ()
	{
		if (!CheckExistedBall ()) {
			ShowPanelSuccess ();

			DestroyAllRocketExist ();
		}
	}

	void OnGUI ()
	{
		if (isPaused) {
//			PauseGameButton ();
		}
	}

	public void PauseGameButton ()
	{
		Time.timeScale = 0f;
//		coinSpawer.SetActive (false);
//		enemyFlySpawer.SetActive (false);
	
	}

	//	public void ReusmeGameButton ()
	//	{
	//		Time.timeScale = 1f;
	//		pausePanel.SetActive (false);
	//		txtPoint.gameObject.SetActive (true);
	//		coinSpawer.SetActive (true);
	//		enemyFlySpawer.SetActive (true);
	//		txtScore.gameObject.SetActive (true);
	//	}

	//	public void RestartGame ()
	//	{
	//		GlobalValue.AllSpeedIncrementGround = GlobalValue.START_SPEED_GROUND;
	//
	//		Time.timeScale = 1f;
	//		SceneFader.instance.FadeIn ("Main");
	//	}

	//	public void PandaDiedShowPanel (float score, int coin)
	//	{
	//		pausePanel.SetActive (true);
	//		txtPoint.gameObject.SetActive (false);
	//		coinSpawer.SetActive (false);
	//		enemyFlySpawer.SetActive (false);
	//		txtScore.gameObject.SetActive (false);
	//
	//		if (score > ScoreController.instance.GetHighScore ()) {
	//			ScoreController.instance.SetHighScore (score);
	//		}
	//		txtHighScore.text = ScoreController.instance.GetHighScore ().ToString ("0.0");
	//
	//		//Coin
	//		int currentPoint = ScoreController.instance.GetPointsCount ();
	//		ScoreController.instance.SetPointsCount (coin + currentPoint);
	//
	//		txtTotalPoint.text = ScoreController.instance.GetPointsCount ().ToString ("0");
	//
	//		UpdateImgMedal (ScoreController.instance.GetHighScore ());
	//
	//		resumeButton.onClick.RemoveAllListeners ();
	//		resumeButton.onClick.AddListener (() => RestartGame ());
	//	}

	//	public void MenuButton ()
	//	{
	//		Time.timeScale = 1f;
	//		SceneFader.instance.FadeIn ("Start");
	//	}
	//
	//	public void InscreamentScore (float score)
	//	{
	//		txtScore.text = score.ToString ("0.0");
	//
	//		UpdateImgMedal (score);
	//	}
	//
	//	public void InscreamentPoint (int coin)
	//	{
	//		txtPoint.text = coin.ToString ("0");
	//	}

	//	public void UpdateImgMedal (float score)
	//	{
	//		if (score < 20f) {
	////			imgMedal.sprite = medalList [0];
	//
	//		} else if (score >= 20f && score < 60f) {
	////			imgMedal.sprite = medalList [1];
	//			GlobalValue.AllSpeedIncrementGround = GlobalValue.NORMAL_SPEED_GROUND;
	//
	//		} else if (score >= 60f && score < 100f) {
	////			imgMedal.sprite = medalList [2];
	//			GlobalValue.AllSpeedIncrementGround = GlobalValue.NORMAL1_SPEED_GROUND;
	//
	//		} else if (score >= 100f && score < 150f) {
	////			imgMedal.sprite = medalList [3];
	//			GlobalValue.AllSpeedIncrementGround = GlobalValue.HARD_SPEED_GROUND;
	//
	//		} else if (score >= 150f && score < 210f) {
	////			imgMedal.sprite = medalList [4];
	//			GlobalValue.AllSpeedIncrementGround = GlobalValue.HARD_SPEED_GROUND;
	//
	//		} else if (score >= 210f && score < 270f) {
	////			imgMedal.sprite = medalList [5];
	//			GlobalValue.AllSpeedIncrementGround = GlobalValue.VERY_HARD_SPEED_GROUND;
	//
	//		} else if (score >= 270 && score < 350f) {
	////			imgMedal.sprite = medalList [6];
	//			GlobalValue.AllSpeedIncrementGround = GlobalValue.VERY_HARD_SPEED_GROUND;
	//
	//		} else if (score >= 350f && score < 400f) {
	////			imgMedal.sprite = medalList [7];
	//			GlobalValue.AllSpeedIncrementGround = GlobalValue.EXTREMLY_HARD_SPEDD_GROUND;
	//
	//		} else if (score >= 400f) {
	////			imgMedal.sprite = medalList [8];
	//			GlobalValue.AllSpeedIncrementGround = GlobalValue.EXTREMLY_HARD_SPEDD_GROUND;
	//		}
	//	}

	//Pause game
	void OnApplicationFocus (bool hasFocus)
	{
		isPaused = !hasFocus;
	}

	void OnApplicationPause (bool pauseStatus)
	{
		isPaused = pauseStatus;
	}


	public void ShowPanelSuccess ()
	{
		Time.timeScale = 0f;

		panelSuccess.SetActive (true);
		inactivePlayer.SetActive (false);

		txtLevelSuccess.text = getNextLevel () + "";
	}

	private bool CheckExistedBall ()
	{
		GameObject[] gos = (GameObject[])FindObjectsOfType (typeof(GameObject));
//		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Ball");
		for (int i = 0; i < gos.Length; i++) {
			if (gos [i].name.Contains ("Ball"))
				return true;
		}

		return false;
	}

	public void DestroyAllRocketExist ()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Rocket");
		for (int i = 0; i < gos.Length; i++) {
			Destroy (gos [i]);
		}
	}

	private int getNextLevel ()
	{
		string[] arrNameSceneCurrent = SceneManager.GetActiveScene ().name.Split ("_" [0]);

		return (int.Parse (arrNameSceneCurrent [2]) + 1);
	}
}



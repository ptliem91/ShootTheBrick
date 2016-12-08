using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{

	public static GameplayController instance;

	[SerializeField]
	private GameObject[] topAndBottomBricks, leftBricks, rightBricks;

	public GameObject panelBG, levelFinishedPanel, playerDiedPanel, pausePanel;

	private GameObject topBrick, bottomBrick, leftBrick, rightBrick;

	private Vector3 coordinates;

	[SerializeField]
	private GameObject[] players;

	public float levelTime;

	public Text liveText, scoreText, levelTimerText, showScoreAtEndOfLevelText, countDownAndBeginLevelText, watchVideoText;

	private float countDownBeforeLevelBegins = 2.0f;

	public static int smallBallsCount = 0;

	public int playerLives, playerScore, coins;

	public bool levelInProgress;

	private bool isGamePaused, hasLevelBegan, countDownLevel;


	[SerializeField]
	private GameObject[] endOfLevelRewards;

	[SerializeField]
	private Button pauseBtn;

	[SerializeField]
	private Color[] colors;

	private float changeColorTime;

	void Awake ()
	{
		MakeInstance ();
		InitializeBricksAndPlayer ();	
	}

	void Start ()
	{
		changeColorTime = levelTime;
		
		InitializeGameplayController ();
	}

	void Update ()
	{
		UpdateGameplayController ();
	}

	void MakeInstance ()
	{
		if (instance == null) {
			instance = this;
		}
	}

	void InitializeBricksAndPlayer ()
	{
		coordinates = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, 0));

		int index = Random.Range (0, topAndBottomBricks.Length);

		topBrick = Instantiate (topAndBottomBricks [index]);
		topBrick.tag = "TopBrick";

		bottomBrick = Instantiate (topAndBottomBricks [index]);
		leftBrick = Instantiate (leftBricks [index], new Vector3 (0, 0, 0), Quaternion.Euler (new Vector3 (0, 0, -90))) as GameObject;
		rightBrick = Instantiate (rightBricks [index], new Vector3 (0, 0, 0), Quaternion.Euler (new Vector3 (0, 0, 90))) as GameObject;
			
		topBrick.transform.position = new Vector3 (-coordinates.x + 8, coordinates.y + 0.35f, 0);
		bottomBrick.transform.position = new Vector3 (-coordinates.x + 8, -coordinates.y - 0.35f, 0);
		leftBrick.transform.position = new Vector3 (-coordinates.x - 0.35f, coordinates.y, 0);
		rightBrick.transform.position = new Vector3 (coordinates.x + 0.35f, coordinates.y, 0);
	
		Instantiate (players [GameController.instance.selectedPlayer]);
	}

	void InitializeGameplayController ()
	{
		if (GameController.instance.isGameStaredFromLevelMenu) {
			playerScore = 0;
			playerLives = 2;
			GameController.instance.currentScore = playerScore;
			GameController.instance.currentLives = playerLives;
			GameController.instance.isGameStaredFromLevelMenu = false;

		} else {
			playerScore = GameController.instance.currentScore;
			playerLives = GameController.instance.currentLives;
		}

		levelTimerText.text = levelTime.ToString ("F0");
		scoreText.text = "Score x" + playerScore;
		liveText.text = "" + GameController.instance.currentLives;

		Time.timeScale = 0;
		countDownAndBeginLevelText.text = countDownBeforeLevelBegins.ToString ("F0");
	}

	void UpdateGameplayController ()
	{
		scoreText.text = "Score x" + playerScore;

		if (hasLevelBegan) {
			CountdownAndBeginLevel ();
		}

		if (countDownLevel) {
			LevelCountdownTimer ();
		}
	}

	public void setHasLevelBegan (bool hasLevelBegan)
	{
		this.hasLevelBegan = hasLevelBegan;
	}

	void CountdownAndBeginLevel ()
	{
		countDownBeforeLevelBegins -= (0.19f * 0.15f);
		countDownAndBeginLevelText.text = countDownBeforeLevelBegins.ToString ("F0");

		if (countDownBeforeLevelBegins <= 0) {
			Time.timeScale = 1f;
			hasLevelBegan = false;
			levelInProgress = true;
			countDownLevel = true;
			countDownAndBeginLevelText.gameObject.SetActive (false);
		}
	}

	void LevelCountdownTimer ()
	{
		if (Time.timeScale == 1) {
			levelTime -= Time.deltaTime;
			levelTimerText.text = levelTime.ToString ("F0");

			if (levelTime < changeColorTime) {
				levelTimerText.color = colors [Random.Range (0, colors.Length)];
				changeColorTime -= 5;
			}

			if (levelTime <= 0) {
				playerLives--;
				GameController.instance.currentLevel = playerLives;
				GameController.instance.currentScore = playerScore;
			
				if (playerLives <= 0) {
					//prompt the user to watch video
					StartCoroutine (IEPromptTheUserToWatchAVideo ());
				} else {
					//restart game
					StartCoroutine (IEPlayerDiedRestartLevel ());
				}
			
			} else {
				
			}
		}
	}

	IEnumerator IEPlayerDiedRestartLevel ()
	{
		levelInProgress = false;

		coins = 0;
		smallBallsCount = 0;

		Time.timeScale = 0f;
		//fade out
		if (LoadingScreenScript.instance != null) {
			LoadingScreenScript.instance.FadeOut ();
		}

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (.5f));

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

		if (LoadingScreenScript.instance != null) {
			LoadingScreenScript.instance.PlayFadeInAnimation ();
		}
	}

	public void PlayerDied ()
	{
		countDownLevel = false;
		pauseBtn.interactable = false;
		levelInProgress = false;

		smallBallsCount = 0;

		playerLives--;
		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		if (playerLives <= 0) {
			//prompt the user to watch video
			StartCoroutine (IEPromptTheUserToWatchAVideo ());
		} else {
			//restart game
			StartCoroutine (IEPlayerDiedRestartLevel ());
		}
	}

	IEnumerator IELevelCompleted ()
	{
		countDownLevel = false;
		pauseBtn.interactable = false;

		int unlockedLevel = GameController.instance.currentLevel;
		unlockedLevel++;

		if (!(unlockedLevel >= GameController.instance.levels.Length)) {
			GameController.instance.levels [unlockedLevel] = true;
			GameController.instance.Save ();
		}

//		Instantiate (endOfLevelRewards [GameController.instance.currentLevel],
//			new Vector3 (0, Camera.main.orthographicSize, 0), Quaternion.identity);

		if (GameController.instance.doubleCoins) {
			coins *= 2;
		}

		GameController.instance.coins = coins;
		GameController.instance.Save ();

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (4f));
		levelInProgress = false;
		Player.instance.StopMoving ();
		Time.timeScale = 0f;

		levelFinishedPanel.SetActive (true);
		showScoreAtEndOfLevelText.text = "" + playerScore;

	}

	IEnumerator IEPromptTheUserToWatchAVideo ()
	{
		levelInProgress = false;
		countDownLevel = false;
		pauseBtn.interactable = false;

		Time.timeScale = 0f;

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (.8f));

		playerDiedPanel.SetActive (true);
	}

	IEnumerator IEGivePlayerLivesAfterWatchingVideo ()
	{
		watchVideoText.text = "Thank you for watching. You earned 2 extra lives.";

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (2.5f));

		coins = 0;
		playerLives = 2;
		smallBallsCount = 0;

		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		Time.timeScale = 0f;

		if (LoadingScreenScript.instance != null) {
			LoadingScreenScript.instance.FadeOut ();
		}

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (1.25f));

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

		if (LoadingScreenScript.instance != null) {
			LoadingScreenScript.instance.PlayFadeInAnimation ();
		}
	}

	public void CountSmallBalls ()
	{
		smallBallsCount--;

		if (smallBallsCount == 0) {
			StartCoroutine (IELevelCompleted ());
		}
	}

	public void GoToSelectLevelButton ()
	{
		GameController.instance.currentScore = playerScore;

		if (GameController.instance.highScore < GameController.instance.currentScore) {
			GameController.instance.highScore = GameController.instance.currentScore;
			GameController.instance.Save ();
		}

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}

		SceneManager.LoadScene ("GP_Lvl_Select");

		if (LoadingScreenScript.instance != null) {
			LoadingScreenScript.instance.PlayLoadingScreen ();
		}
	}

	public void RestartCurrentLevelButton ()
	{
		smallBallsCount = 0;
		coins = 0;

		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

		if (LoadingScreenScript.instance != null) {
			LoadingScreenScript.instance.PlayLoadingScreen ();
		}
	}

	public void NextLevelButton ()
	{
		GameController.instance.currentLives = playerLives;
		GameController.instance.currentScore = playerScore;

		if (GameController.instance.highScore < GameController.instance.currentScore) {
			GameController.instance.highScore = GameController.instance.currentScore;
			GameController.instance.Save ();
		}

		int nextLevel = GameController.instance.currentLevel;
		nextLevel++;

		if (nextLevel < GameController.instance.levels.Length) {
			GameController.instance.currentLevel = nextLevel;

			SceneManager.LoadScene ("GP_Lvl_" + nextLevel);

			if (LoadingScreenScript.instance != null) {
				LoadingScreenScript.instance.PlayLoadingScreen ();
			}
		}
	}

	public void PauseGameButton ()
	{
		if (!hasLevelBegan) {
			if (levelInProgress) {
				if (!isGamePaused) {
					countDownLevel = false;
					levelInProgress = false;
					isGamePaused = true;

					panelBG.SetActive (true);
					pausePanel.SetActive (true);

					Time.timeScale = 0f;
				}
			}
		}
	}

	public void ResumeGameButton ()
	{
		countDownLevel = true;
		levelInProgress = true;
		isGamePaused = false;

		panelBG.SetActive (false);
		pausePanel.SetActive (false);

		Time.timeScale = 1f;
	}

	public void DontWatchAVideoAndQuit ()
	{
		GameController.instance.currentScore = playerScore;

		if (GameController.instance.highScore < GameController.instance.currentScore) {
			GameController.instance.highScore = GameController.instance.currentScore;
			GameController.instance.Save ();
		}

		Time.timeScale = 1f;

		SceneManager.LoadScene ("GP_Lvl_Select");

		if (LoadingScreenScript.instance != null) {
			LoadingScreenScript.instance.PlayLoadingScreen ();
		}
	}

	public void VideoNotLoadedOrUserSkippedTheVideo (string message)
	{
		watchVideoText.text = message;
	}

	public void VideoWatchedGivePlayerLives ()
	{
		StartCoroutine (IEGivePlayerLivesAfterWatchingVideo ());
	}

	public void WatchVideoToEarnExtralives ()
	{
//		AdsController.instance.ShowUnityAds ();
	}
}





























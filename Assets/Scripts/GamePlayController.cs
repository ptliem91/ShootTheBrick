using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class GamePlayController : MonoBehaviour
{
	public static GamePlayController instance;

	[SerializeField]
	private GameObject panelSuccess;

	[SerializeField]
	private Text txtLevelSuccess;

	[SerializeField]
	private Button btnPause;

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
	}

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
		btnPause.gameObject.SetActive (false);

		txtLevelSuccess.text = getNextLevel () + "";

		PlayerPrefs.SetInt ("CurrentLevel", getNextLevel ());
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

		return (GetCurrentLevel () + 1);
	}

	private int GetCurrentLevel ()
	{
		return int.Parse (SceneManager.GetActiveScene ().name.Split ("_" [0]) [2]);
	}
}



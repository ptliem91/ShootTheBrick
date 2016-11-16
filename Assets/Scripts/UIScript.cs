using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
	[SerializeField]
	private Button btnPause;

	[SerializeField]
	private GameObject panelPause, panelFailed;

	public void PauseGameButton ()
	{
		Time.timeScale = 0f;

		panelPause.SetActive (true);
		btnPause.gameObject.SetActive (false);
	}

	public void ResumeGameButton ()
	{
		Time.timeScale = 1f;

		panelPause.SetActive (false);
		btnPause.gameObject.SetActive (true);
	}

	public void RestartGame ()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void NextLevel ()
	{
		string[] arrNameSceneCurrent = SceneManager.GetActiveScene ().name.Split ("_" [0]);

		int nextLevel = int.Parse (arrNameSceneCurrent [2]) + 1;

		string newScence = arrNameSceneCurrent [0] + "_" + arrNameSceneCurrent [1] + "_" + nextLevel;

		SceneManager.LoadScene (newScence);
	}

	public void FailedGame ()
	{
		StartCoroutine (WaitingFailedGame ());
	}

	private void HiddenPlayerAndAllBalls ()
	{
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		player.SetActive (false);

		GameObject[] arrLargeBall = GameObject.FindGameObjectsWithTag ("Large Ball");
		for (int i = 0; i < arrLargeBall.Length; i++) {
			arrLargeBall [i].SetActive (false);
		}

		GameObject[] arrMediumBall = GameObject.FindGameObjectsWithTag ("Medium Ball");
		for (int i = 0; i < arrMediumBall.Length; i++) {
			arrMediumBall [i].SetActive (false);
		}

		GameObject[] arrSmallBall = GameObject.FindGameObjectsWithTag ("Small Ball");
		for (int i = 0; i < arrSmallBall.Length; i++) {
			arrSmallBall [i].SetActive (false);
		}
	}

	IEnumerator WaitingFailedGame ()
	{
		GamePlayController.instance.DestroyAllRocketExist ();

		yield return new WaitForSeconds (1.5f);

		panelFailed.SetActive (true);
//		HiddenPlayerAndAllBalls ();

		Time.timeScale = 0f;
	}

	public void showPanelLevelSelect ()
	{
//		SceneManager.LoadScene ("GP_Lvl_Menu");
		Application.LoadLevel (0);
	}
}

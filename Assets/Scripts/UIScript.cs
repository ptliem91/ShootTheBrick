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
		panelFailed.SetActive (true);
	}
}

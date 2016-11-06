using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
	[SerializeField]
	private Button btnPause;

	[SerializeField]
	private GameObject panelPause;

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
}

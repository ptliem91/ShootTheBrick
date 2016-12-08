using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuitGameController : MonoBehaviour
{

	public static QuitGameController instance;

	[SerializeField]
	private GameObject quitPanel;

	void Awake ()
	{
		MakeSingleton ();
	}

	void Update ()
	{
		if (Input.GetKey (KeyCode.Escape)) {
			quitPanel.SetActive (true);
		}
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

	public void QuitGameButton ()
	{
		StartCoroutine (IEQuitGame ());
	}

	public void CancelQuit ()
	{
		quitPanel.SetActive (false);
	}

	IEnumerator IEQuitGame ()
	{
		GameController.instance.Save ();

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (0.5f));

		Application.Quit ();
	}
}

using UnityEngine;
using System.Collections;

public class LoadingScreenScript : MonoBehaviour
{
	public static LoadingScreenScript instance;

	[SerializeField]
	private GameObject bgImage, nameGameText, loadingText, fadePanel;

	[SerializeField]
	private Animator fadeAnim;

	void Awake ()
	{
		MakeSingleton ();
		Hide ();
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

	public void PlayLoadingScreen ()
	{
		StartCoroutine (IEShowLoadingScreen ());
	}

	public void PlayFadeInAnimation ()
	{
		StartCoroutine (FadeIn ());
	}

	IEnumerator FadeIn ()
	{
		fadeAnim.Play ("FadeIn");
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (.5f));

		if (GameplayController.instance != null) {
			GameplayController.instance.setHasLevelBegan (true);
		}

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (.5f));
		fadePanel.SetActive (false);
	}

	public void FadeOut ()
	{
		fadePanel.SetActive (true);
		fadeAnim.Play ("FadeOut");
	}

	IEnumerator IEShowLoadingScreen ()
	{
		Show ();
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (1f));
		Hide ();

		if (GameplayController.instance != null) {
			GameplayController.instance.setHasLevelBegan (true);
		}
	}

	void Show ()
	{
		bgImage.SetActive (true);
		nameGameText.SetActive (true);
		loadingText.SetActive (true);
	}

	void Hide ()
	{
		bgImage.SetActive (false);
		nameGameText.SetActive (false);
		loadingText.SetActive (false);
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{

	public static MusicController instance;

	private AudioSource bgMusic, click;

	private float time;

	void Awake ()
	{
		MakeSingleton ();

		AudioSource[] audioSources = GetComponents<AudioSource> ();

		bgMusic = audioSources [0];
		click = audioSources [1];
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

	void OnLevelWasLoaded ()
	{
		if (SceneManager.GetActiveScene ().name == "GP_Lvl_Select") {
			if (GameController.instance.isMusicOn) {
				if (!bgMusic.isPlaying) {
					bgMusic.time = time;
					bgMusic.Play ();
				}
			}
		}
	}

	public void GameIsLoadedTurnOfMusic ()
	{
		if (bgMusic.isPlaying) {
			time = bgMusic.time;
			bgMusic.Stop ();
		}
	}

	public void PlayBgMusic ()
	{
		if (!bgMusic.isPlaying) {
			bgMusic.Play ();
		}
	}

	public void StopBgMusic ()
	{
		if (bgMusic.isPlaying) {
			bgMusic.Stop ();
		}
	}

	public void PlayClickClip ()
	{
		click.Play ();
	}

}

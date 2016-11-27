using UnityEngine;
using System.Collections;

public class SoundSetting : MonoBehaviour
{
	[SerializeField]
	private GameObject audioOnIcon, audioOffIcon;

	void Awake ()
	{
		SetSoundState ();
	}

	public void ToggleSound ()
	{
		if (PlayerPrefs.GetInt ("Muted", 0) == 0) {
			PlayerPrefs.SetInt ("Muted", 1);
		} else {
			PlayerPrefs.SetInt ("Muted", 0);
		}

		SetSoundState ();
	}

	private void SetSoundState ()
	{
		if (PlayerPrefs.GetInt ("Muted", 0) == 0) {
			AudioListener.volume = 1;
			audioOffIcon.SetActive (false);
			audioOnIcon.SetActive (true);

		} else {
			AudioListener.volume = 0;
			audioOffIcon.SetActive (true);
			audioOnIcon.SetActive (false);
		}
	}
}

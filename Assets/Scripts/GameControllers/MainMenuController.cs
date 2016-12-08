using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	[SerializeField]
	private Animator settingButtonsAnim;

	private bool hidden;
	private bool canTouchSettingButton;

	[SerializeField]
	private Button musicBtn;

	[SerializeField]
	private Sprite[] musicBtnSprites;

	[SerializeField]
	private Button fbBtn;

	[SerializeField]
	private Sprite[] fbSprites;

	void Start ()
	{
		canTouchSettingButton = true;
		hidden = true;

		if (GameController.instance.isMusicOn) {
			MusicController.instance.PlayBgMusic ();
			musicBtn.image.sprite = musicBtnSprites [0];

			AudioListener.volume = 1;

		} else {
			MusicController.instance.StopBgMusic ();
			musicBtn.image.sprite = musicBtnSprites [1];

			AudioListener.volume = 0;
		}
	}

	public void SettingButton ()
	{
		StartCoroutine (DissableSettingButtonWhilePlayingAnimation ()); 
	}

	IEnumerator DissableSettingButtonWhilePlayingAnimation ()
	{
		if (canTouchSettingButton) {
			if (hidden) {
				canTouchSettingButton = false;
				settingButtonsAnim.Play ("SlideIn");
				hidden = false;

				yield return new WaitForSeconds (1.2f);
				canTouchSettingButton = true;
			} else {
				canTouchSettingButton = false;
				settingButtonsAnim.Play ("SlideOut");
				hidden = true;

				yield return new WaitForSeconds (1.2f);
				canTouchSettingButton = true;
			}
		}
	}

	public void MusicButton ()
	{
		if (GameController.instance.isMusicOn) {
			musicBtn.image.sprite = musicBtnSprites [1];

			MusicController.instance.StopBgMusic ();

			GameController.instance.isMusicOn = false;
			GameController.instance.Save ();

			AudioListener.volume = 0;

		} else {
			musicBtn.image.sprite = musicBtnSprites [0];

			MusicController.instance.PlayBgMusic ();

			GameController.instance.isMusicOn = true;
			GameController.instance.Save ();

			AudioListener.volume = 1;
		}
	}

	public void PlayButton ()
	{
		MusicController.instance.PlayClickClip ();

		SceneManager.LoadScene ("GP_Lvl_Select");
	}
}

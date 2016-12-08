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

	// Use this for initialization
	void Start ()
	{
		canTouchSettingButton = true;
		hidden = true;

		if (GameController.instance.isMusicOn) {
			MusicController.instance.PlayBgMusic ();
			musicBtn.image.sprite = musicBtnSprites [0];
		} else {
			MusicController.instance.StopBgMusic ();
			musicBtn.image.sprite = musicBtnSprites [1];
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
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

		} else {
			musicBtn.image.sprite = musicBtnSprites [0];

			MusicController.instance.PlayBgMusic ();

			GameController.instance.isMusicOn = true;
			GameController.instance.Save ();
		}
	}

	public void PlayButton ()
	{
		MusicController.instance.PlayClickClip ();

		SceneManager.LoadScene ("GP_Lvl_Select");
	}
}

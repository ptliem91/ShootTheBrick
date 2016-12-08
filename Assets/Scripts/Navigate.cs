using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Navigate : MonoBehaviour
{

	public void GoToMainMenu ()
	{
		MusicController.instance.PlayClickClip ();
		SceneManager.LoadScene ("GP_Main_Menu");
	}

	public void GoToShopMenu ()
	{
		MusicController.instance.PlayClickClip ();
		SceneManager.LoadScene ("Shop_Menu");
	}

	public void GoToChangeSoulSword ()
	{
		MusicController.instance.PlayClickClip ();
		SceneManager.LoadScene ("GP_SoulSword_Select");
	}
}

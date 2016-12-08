using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Navigate : MonoBehaviour
{

	public void GoToMainMenu ()
	{
		SceneManager.LoadScene ("GP_Main_Menu");
	}

	public void GoToShopMenu ()
	{
		SceneManager.LoadScene ("Shop_Menu");
	}

	public void GoToChangeSoulSword ()
	{
		SceneManager.LoadScene ("GP_SoulSword_Select");
	}
}

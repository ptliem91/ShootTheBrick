using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class ScrollSelectSoulSword : MonoBehaviour
{
	//Public
	public RectTransform panel;
	public Button[] weapons;
	public RectTransform center;
	public int startButton = 1;

	//Private
	public float[] distance;
	public float[] distReposition;

	private bool dragging = false;
	private int bttnDistance;
	private int minButtonNum;

	private bool isButtonSelected = false;

	// Use this for initialization
	void Awake ()
	{
		int bttnLenght = weapons.Length;
		distance = new float[bttnLenght];
		distReposition = new float[bttnLenght];

		//Get distane between buttons
		bttnDistance = (int)Mathf.Abs (weapons [1].GetComponent<RectTransform> ().anchoredPosition.x - weapons [0].GetComponent<RectTransform> ().anchoredPosition.x);

//		print ("Start button::" + GameController.instance.selectedWeapon);
		startButton = GameController.instance.selectedWeapon;

		panel.anchoredPosition = new Vector2 ((startButton - 1) * -300, 0f);

		for (int a = 1; a < weapons.Length; a++) {
//			print ("Panel " + a + ":: " + GameController.instance.weapons [a]);
			if (GameController.instance.weapons [a] == false) {
//				panelWeapons [a].SetActive (false);
				weapons [a].gameObject.SetActive (false);
			}
		}

		if (Time.timeScale == 0) {
			Time.timeScale = 1f;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < weapons.Length; i++) {
			
			distReposition [i] = center.GetComponent<RectTransform> ().position.x - weapons [i].GetComponent<RectTransform> ().position.x;
			distance [i] = Mathf.Abs (distReposition [i]);

		}

		float minDistance = Mathf.Min (distance);

		int weaponCurrent = startButton;

		for (int a = 0; a < weapons.Length; a++) {
			if (minDistance == distance [a]) {
				minButtonNum = a;
				weapons [a].transform.localScale = new Vector2 (1.5f, 1.5f);

			} else {
				weapons [a].transform.localScale = new Vector2 (1f, 1f);
			}
		}

		if (!dragging) {
			
			LerpToBttn (minButtonNum * -bttnDistance);


			for (int a = 0; a < weapons.Length; a++) {
				weapons [a].onClick.RemoveAllListeners ();
			}

			if (GameController.instance.weapons [minButtonNum] == true) {
				weapons [minButtonNum].onClick.AddListener (() => {
					SelectedWeaponAndChangeLevelScence (minButtonNum);
				});
			}
		}
	}

	void LerpToBttn (int position)
	{
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * 15f);

		////zoom
		if ((Mathf.Abs (newX) >= Mathf.Abs (position) - 1f) && (Mathf.Abs (newX) <= Mathf.Abs (position) + 1f) && !isButtonSelected) {			
			isButtonSelected = true;
//			Debug.Log (bttn [minButtonNum].name);
		}

		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		panel.anchoredPosition = newPosition;
	}

	public void StartDrag ()
	{
		dragging = true;
		isButtonSelected = false;
	}

	public void EndDrag ()
	{
		dragging = false;
	}

	void SelectedWeaponAndChangeLevelScence (int swordSelected)
	{
		print ("swordSelected::" + swordSelected);

		GameController.instance.selectedWeapon = swordSelected;
		GameController.instance.Save ();

		LoadingScreenScript.instance.PlayLoadingScreen ();

		if (GameController.instance.weapons [swordSelected]) {
			SceneManager.LoadScene ("GP_Lvl_Select");
		}
	}
}

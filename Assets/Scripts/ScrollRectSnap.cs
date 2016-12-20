using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class ScrollRectSnap : MonoBehaviour
{
	//Public
	public RectTransform panel;
	public RectTransform center;
	public int startButton = 1;
	public Button prefabButton;
	public int numLevelButton;

	//Private
	public float[] distance;
	public float[] distReposition;

	private bool dragging = false;
	private int bttnDistance;
	private int minButtonNum;

	private Button[] bttn;

	private bool isButtonSelected = false;

	// Use this for initialization
	void Awake ()
	{
		//
		int bttnLenght = numLevelButton;
		distance = new float[bttnLenght];
		distReposition = new float[bttnLenght];

		//
		bttn = new Button[bttnLenght];

		//generate level button
		int positionXLvlButon = 0;
		for (int i = 1; i <= numLevelButton; i++) {
			Button lvlButton = (Button)UnityEngine.Object.Instantiate (prefabButton);

			lvlButton.transform.SetParent (panel, false);
			lvlButton.name = "Button_" + i;

			Vector2 temp = panel.anchoredPosition;
			temp.x = positionXLvlButon;
			temp.y = -50;

			lvlButton.transform.localPosition = new Vector3 (temp.x, temp.y, 0);

			Text textLvlButton = lvlButton.GetComponentInChildren<Text> ();
			textLvlButton.text = "" + i;
			if (i > 9) {
				textLvlButton.fontSize = 180;
			}

			positionXLvlButon += 300;

			bttn [i - 1] = lvlButton;
		}
		//

		//Get distane between buttons
		bttnDistance = (int)Mathf.Abs (bttn [1].GetComponent<RectTransform> ().anchoredPosition.x - bttn [0].GetComponent<RectTransform> ().anchoredPosition.x);


		bool[] levels = GameController.instance.levels;

		for (int i = levels.Length - 1; i > 1; i--) {
			if (GameController.instance.currentLevel != -1) {
				startButton = GameController.instance.currentLevel;

			} else if (levels [i]) {
				startButton = i;
				break;
			}
		}
		panel.anchoredPosition = new Vector2 ((startButton - 1) * -300, 0f);

		Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < bttn.Length; i++) {
			
			distReposition [i] = center.GetComponent<RectTransform> ().position.x - bttn [i].GetComponent<RectTransform> ().position.x;
			distance [i] = Mathf.Abs (distReposition [i]);

		}

		float minDistance = Mathf.Min (distance);

		int maxLevelActived = startButton;
		bool[] levels = GameController.instance.levels;
		for (int i = levels.Length - 1; i > 1; i--) {
			if (levels [i]) {
				maxLevelActived = i;
				break;
			}
		}

		for (int a = 0; a < bttn.Length; a++) {
			if (minDistance == distance [a]) {
				minButtonNum = a;
				bttn [a].transform.localScale = new Vector2 (1.6f, 1.6f);

			} else {
				bttn [a].transform.localScale = new Vector2 (1f, 1f);

				Text textBtn = bttn [a].GetComponentInChildren<Text> ();
				if (int.Parse (textBtn.text) > maxLevelActived) {
					textBtn.color = Color.grey;
				}
			}
		}

		if (!dragging) {
			
			LerpToBttn (minButtonNum * -bttnDistance);

			//Button center
//			bttn [minButtonNum].GetComponent<RectTransform> ().sizeDelta = new Vector2 (bttn [minButtonNum].GetComponent<RectTransform> ().sizeDelta.x + 0.5f, bttn [minButtonNum].GetComponent<RectTransform> ().sizeDelta.y + 0.5f);

			for (int a = 0; a < bttn.Length; a++) {
				bttn [a].onClick.RemoveAllListeners ();
			}

			bttn [minButtonNum].onClick.AddListener (() => {
				PlaySence (bttn [minButtonNum], maxLevelActived);
			});
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

	void PlaySence (Button buttonCenter, int maxLevelActived)
	{
		Text textBtn = buttonCenter.GetComponentInChildren<Text> ();
		int lvlSelected = int.Parse (textBtn.text);

		if (lvlSelected == 40) {
			lvlSelected = 1;
		}

//		print ("level current::" + lvlSelected);

		if (lvlSelected <= maxLevelActived) {
			LoadingScreenScript.instance.PlayLoadingScreen ();

			GameController.instance.isGameStaredFromLevelMenu = true;
			GameController.instance.currentLevel = lvlSelected;

			SceneManager.LoadScene ("GP_Lvl_" + lvlSelected.ToString ());
		}
	}
}

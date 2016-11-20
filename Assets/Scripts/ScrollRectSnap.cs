using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollRectSnap : MonoBehaviour
{
	//Public
	public RectTransform panel;
	public Button[] bttn;
	public RectTransform center;
	public int startButton = 1;
	//	public GameObject img;

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
		int bttnLenght = bttn.Length;
		distance = new float[bttnLenght];
		distReposition = new float[bttnLenght];

		//Get distane between buttons
		bttnDistance = (int)Mathf.Abs (bttn [1].GetComponent<RectTransform> ().anchoredPosition.x - bttn [0].GetComponent<RectTransform> ().anchoredPosition.x);

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

		for (int a = 0; a < bttn.Length; a++) {
			if (minDistance == distance [a]) {
				minButtonNum = a;
				bttn [a].transform.localScale = new Vector2 (1.5f, 1.5f);

			} else {
				bttn [a].transform.localScale = new Vector2 (1f, 1f);

				Text textBtn = bttn [a].GetComponentInChildren<Text> ();
				if (int.Parse (textBtn.text) > 4) {
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
				ChangeToSence (bttn [minButtonNum]);
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

	void ChangeToSence (Button buttonCenter)
	{
		Text textBtn = buttonCenter.GetComponentInChildren<Text> ();
		if (int.Parse (textBtn.text) <= 4) {
			string[] arrBttnName = buttonCenter.name.Split ("_" [0]);

			SceneManager.LoadScene ("GP_Lvl_" + arrBttnName [1]);
		}
	}
}

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
	void Start ()
	{
		int bttnLenght = bttn.Length;
		distance = new float[bttnLenght];
		distReposition = new float[bttnLenght];

		//Get distane between buttons
		bttnDistance = (int)Mathf.Abs (bttn [1].GetComponent<RectTransform> ().anchoredPosition.x - bttn [0].GetComponent<RectTransform> ().anchoredPosition.x);

		panel.anchoredPosition = new Vector2 ((startButton - 1) * -300, 0f);
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
			}
		}

		if (!dragging) {
			LerpToBttn (minButtonNum * -bttnDistance);

			//Button center
//			bttn [minButtonNum].GetComponent<RectTransform> ().sizeDelta = new Vector2 (bttn [minButtonNum].GetComponent<RectTransform> ().sizeDelta.x + 0.5f, bttn [minButtonNum].GetComponent<RectTransform> ().sizeDelta.y + 0.5f);
				
//			Button btn = bttn [i].GetComponent<Button> ();
			bttn [minButtonNum].onClick.AddListener (() => {
				ChangeSence (bttn [minButtonNum].name);
			});

		}
	}

	void LerpToBttn (int position)
	{
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * 5f);

		//zoom
		if ((Mathf.Abs (newX) >= Mathf.Abs (position) - 1f) && (Mathf.Abs (newX) <= Mathf.Abs (position) + 1f) && !isButtonSelected) {

//			img.SetActive (true);
//			bttn [minButtonNum].GetComponent<RectTransform> ().sizeDelta = new Vector2 (bttn [minButtonNum].GetComponent<RectTransform> ().sizeDelta.x * 1.5f,
//				bttn [minButtonNum].GetComponent<RectTransform> ().sizeDelta.y * 1.5f);
			
			isButtonSelected = true;

			Debug.Log (bttn [minButtonNum].name);
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

	void ChangeSence (string buttonName)
	{
		string[] arrBttnName = buttonName.Split ("_" [0]);

		SceneManager.LoadScene ("GP_Lvl_" + arrBttnName [1]);
	}
}

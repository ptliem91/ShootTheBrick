using UnityEngine;
using System.Collections;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class TapToMove : MonoBehaviour
{
	private float speed = 200f;

	[SerializeField]
	private GameObject rocket;

	//inside class
	private Vector2 firstPressPos;
	private Vector2 secondPressPos;
	private Vector2 currentSwipe;

	private Rigidbody2D myBody;
	private Animator anim;

	// Use this for initialization
	void Awake ()
	{
		myBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate ()
	{
		#if UNITY_EDITOR
		MouseDrag ();

		#elif UNITY_ANDROID
		Swipe();

		#else
		Debug.Log("Any other platform");

		#endif
	}

	public void Swipe ()
	{
		var force = 0f;

		if (Input.touches.Length > 0) {
			Touch t = Input.GetTouch (0);
			if (t.phase == TouchPhase.Began) {
				//save began touch 2d point
				firstPressPos = new Vector2 (t.position.x, t.position.y);
			}
			if (t.phase == TouchPhase.Ended) {
				//save ended touch 2d point
				secondPressPos = new Vector2 (t.position.x, t.position.y);

				//create vector from the two points
				currentSwipe = new Vector3 (secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

				//normalize the 2d vector
				currentSwipe.Normalize ();

				//swipe upwards
				if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					Debug.Log ("up swipe");
				}
				//swipe down
				if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					Debug.Log ("down swipe");
				}
				//swipe left
				if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					Debug.Log ("left swipe");

					Vector3 scale = transform.localScale;
					scale.x = 0.55f;

					force = -speed;

					transform.localScale = scale;

					anim.SetBool ("HAppear", false);
					anim.SetBool ("HWalking", true);
				}
				//swipe right
				if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					Debug.Log ("right swipe");

					Vector3 scale = transform.localScale;
					scale.x = -0.55f;

					force = speed;

					transform.localScale = scale;

					anim.SetBool ("HAppear", false);
					anim.SetBool ("HWalking", true);
				}

				myBody.AddRelativeForce (new Vector3 (force, 0, 0));
			}
		}
	}

	private void MouseDrag ()
	{
		var force = 0f;

		if (Input.GetMouseButtonDown (0)) {
			//save began touch 2d point
			firstPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		}
		if (Input.GetMouseButtonUp (0)) {
			//save ended touch 2d point
			secondPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

			//create vector from the two points
			currentSwipe = new Vector2 (secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

			//normalize the 2d vector
			currentSwipe.Normalize ();

			//swipe upwards
			if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
				Debug.Log ("up swipe");
			}
			//swipe down
			if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
				Debug.Log ("down swipe");
			}
			//swipe left
			if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
				Debug.Log ("left swipe");

				Vector3 scale = transform.localScale;
				scale.x = 0.55f;

				//Weapon effect
				Vector3 weaponEffectScale = rocket.transform.localScale;
				weaponEffectScale.x = -0.25f;
				rocket.transform.localScale = weaponEffectScale;

				force = -speed;

				transform.localScale = scale;

				anim.SetBool ("HAppear", false);
				anim.SetBool ("HWalking", true);

			}
			//swipe right
			if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
				Debug.Log ("right swipe");

				Vector3 scale = transform.localScale;
				scale.x = -0.55f;

				//Weapon effect
				Vector3 weaponEffectScale = rocket.transform.localScale;
				weaponEffectScale.x = 0.25f;
				rocket.transform.localScale = weaponEffectScale;

				force = speed;

				transform.localScale = scale;

				anim.SetBool ("HAppear", false);
				anim.SetBool ("HWalking", true);
			}

			print ("Force" + force);
			myBody.AddRelativeForce (new Vector3 (force, 0, 0));
		}
	}
}
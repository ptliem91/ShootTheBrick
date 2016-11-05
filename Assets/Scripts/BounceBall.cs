using UnityEngine;
using System.Collections;

public class BounceBall : MonoBehaviour
{
	public bool moveLeft = false;

	public float forceX = 3f;

	private Rigidbody2D myBody;

	// Use this for initialization
	void Awake ()
	{
		myBody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveBall (moveLeft);
	}

	void MoveBall (bool isMoveLeft)
	{
		if (moveLeft) {
			Vector3 temp = transform.position;
			temp.x -= forceX * Time.deltaTime;
			transform.position = temp;
		}

		if (!isMoveLeft) {
			Vector3 temp = transform.position;
			temp.x += forceX * Time.deltaTime;
			transform.position = temp;
		}
	}

	public void SetMoveLeft (bool canMoveLeft)
	{
		this.moveLeft = canMoveLeft;
	}

	void OnTriggerEnter2D (Collider2D target)
	{
		if (target.tag == "Ground") {
			Destroy (gameObject);
		}

		if (target.tag == "Right Wall") {
			SetMoveLeft (true);
		}

		if (target.tag == "Left Wall") {
			SetMoveLeft (false);
		}

//		if (target.tag == "Rocket") {
//			if (gameObject.tag != "Smallest Ball") {
//				InitializeBallsAndTurnOffCurrentBall ();
//
//				Instantiate (ballParticle, transform.position, Quaternion.identity);
//			} else {
//				AudioSource.PlayClipAtPoint (popSounds [Random.Range (0, popSounds.Length)], transform.position);
//				gameObject.SetActive (false);
//			}
//		}
	}
}

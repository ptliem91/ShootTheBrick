using UnityEngine;
using System.Collections;

public class RocketBounce : MonoBehaviour
{

	public float ballInitialVelocity = 500f;

	private Rigidbody2D rb;
	private bool ballInPlay;

	// Use this for initialization
	void Awake ()
	{
		rb = GetComponent<Rigidbody2D> ();

	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Fire1") && ballInPlay == false) {
			transform.parent = null;
			ballInPlay = true;
			rb.isKinematic = false;
			rb.AddForce (new Vector3 (0, ballInitialVelocity, 0));
		}
	}
}

﻿using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

	private float forceX, forceY;

	private Rigidbody2D myBody;

	[SerializeField]
	private bool moveLeft, moveRight;

	[SerializeField]
	private GameObject originalBall;

	private GameObject ball1, ball2;
	private Ball ball1Script, ball2Script;

	[SerializeField]
	private AudioClip[] popSounds;

	[SerializeField]
	private GameObject ballParticle;

	// Use this for initialization
	void Awake ()
	{
		myBody = GetComponent<Rigidbody2D> ();
		SetBallSpeed ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveBall ();
	}

	void InstantiateBalls ()
	{
		if (this.gameObject.tag != "Smallest Ball") {
			ball1 = Instantiate (originalBall);
			ball2 = Instantiate (originalBall);

			ball1.name = originalBall.name;
			ball2.name = originalBall.name;

			ball1Script = ball1.GetComponent<Ball> ();
			ball2Script = ball2.GetComponent<Ball> ();
		}
	}

	void InitializeBallsAndTurnOffCurrentBall ()
	{
		InstantiateBalls ();

		Vector3 temp = transform.position;

		ball1.transform.position = temp;
		ball1Script.SetMoveLeft (true);

		ball2.transform.position = temp;
		ball2Script.SetMoveRight (true);

		ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2.5f);
		ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2.5f);

		AudioSource.PlayClipAtPoint (popSounds [Random.Range (0, popSounds.Length)], transform.position);
		gameObject.SetActive (false);
	
	}

	public void SetMoveLeft (bool canMoveLeft)
	{
		this.moveLeft = canMoveLeft;
		this.moveRight = !canMoveLeft;
	}

	public void SetMoveRight (bool canMoveRight)
	{
		this.moveRight = canMoveRight;
		this.moveLeft = !canMoveRight;
	}

	void MoveBall ()
	{
		if (moveLeft) {
			Vector3 temp = transform.position;
			temp.x -= forceX * Time.deltaTime;
			transform.position = temp;
		}

		if (moveRight) {
			Vector3 temp = transform.position;
			temp.x += forceX * Time.deltaTime;
			transform.position = temp;
		}
	}

	void SetBallSpeed ()
	{
//		forceX = 2.5f;
		forceX = Random.Range (1, 4) + 0.5f;
		print (forceX);

		switch (this.gameObject.tag) {
		case "Largest Ball":
			forceY = 11.5f;
			break;

		case "Large Ball":
			forceY = 10.5f;
			break;

		case "Medium Ball":
			forceY = 9f;
			break;

		case "Small Ball":
			forceY = 8f;
			break;

		case "Smallest Ball":
			forceY = 7f;
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D target)
	{
		if (target.tag == "Ground") {
			myBody.velocity = new Vector2 (0, forceY);
		}

		if (target.tag == "Right Wall") {
			SetMoveLeft (true);
		}

		if (target.tag == "Left Wall") {
			SetMoveRight (true);
		}

		if (target.tag == "Rocket") {
			GameObject particleSys = (GameObject)Instantiate (ballParticle, transform.position, Quaternion.identity);

			if (gameObject.tag != "Small Ball") {
				InitializeBallsAndTurnOffCurrentBall ();

			} else {
				AudioSource.PlayClipAtPoint (popSounds [Random.Range (0, popSounds.Length)], transform.position);
				gameObject.SetActive (false);
			}

			Destroy (particleSys, 3f);
		}
	}

}

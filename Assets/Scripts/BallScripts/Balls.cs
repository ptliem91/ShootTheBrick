using UnityEngine;
using System.Collections;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class Balls : MonoBehaviour
{
	public int numberBallsGenerate = 2;

	private float forceX, forceY;

	[SerializeField]
	private Rigidbody2D myBody;

	[SerializeField]
	private bool moveLeft, moveRight;

	[SerializeField]
	private GameObject originalBall;

	private GameObject[] arrBalls;
	private Balls[] arrBallScripts;

	[SerializeField]
	private AudioClip impactSound, explosionSound;

	[SerializeField]
	private GameObject explosionParticleStone;

	[SerializeField]
	private GameObject coin;

	private bool isInitFromBoss = false;

	// Use this for initialization
	void Awake ()
	{
		if (this.gameObject.tag == "SmallBall") {
			GameplayController.smallBallsCount++;
		}

		arrBalls = new GameObject[numberBallsGenerate];
		arrBallScripts = new Balls[numberBallsGenerate];

		SetBallSpeed ();
		InstantiateBalls ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveBall ();
		if (isInitFromBoss) {
			SetBallSpeed ();
		}
	}

	void InstantiateBalls ()
	{
//		if (this.gameObject.tag != "SmallBall") {
//			ball1 = Instantiate (originalBall);
//			ball2 = Instantiate (originalBall);
//
//			ball1Script = ball1.GetComponent<Ball> ();
//			ball2Script = ball2.GetComponent<Ball> ();
//
//			ball1.SetActive (false);
//			ball2.SetActive (false);
//		}

		if (this.gameObject.tag != "SmallBall") {
			for (int i = 0; i < numberBallsGenerate; i++) {
				arrBalls [i] = Instantiate (originalBall);
				arrBallScripts [i] = arrBalls [i].GetComponent<Balls> ();
				arrBalls [i].SetActive (false);
			}
		}
	}

	void InitializeBallsAndTurnOffCurrentBall ()
	{

		Vector3 temp = transform.position;

		bool moveLeft = true;

		float forcePX = Random.Range (1f, 4f);
		float forcePXTemp = forcePX;

		for (int i = 0; i < numberBallsGenerate; i++) {
			arrBalls [i].transform.position = temp;

			if (i != 0) {
				while (forcePX == forcePXTemp) {
					forcePX = Random.Range (1f, 4f);
				}

				forcePXTemp = forcePX;
			}

			if (moveLeft) {
				arrBallScripts [i].forceX = forcePX;
				arrBallScripts [i].SetMoveLeft (true);
			} else {
				arrBallScripts [i].forceX = forcePX;
				arrBallScripts [i].SetMoveRight (true);
			}


			moveLeft = !moveLeft;

			arrBalls [i].SetActive (true);

			if (gameObject.tag != "SmallBall") {
				if (transform.position.y > 1 && transform.position.y <= 1.3f) {
					arrBalls [i].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
				} else if (transform.position.y > 1.3f) {
					arrBalls [i].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
				} else if (transform.position.y < 1) {
					arrBalls [i].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
				}
			}
		}

//		ball1.transform.position = temp;
//		ball1Script.SetMoveLeft (true);
//
//		ball2.transform.position = temp;
//		ball2Script.SetMoveRight (true);
//
//		ball1.SetActive (true);
//		ball2.SetActive (true);
//
//		if (gameObject.tag != "SmallBall") {
//			if (transform.position.y > 1 && transform.position.y <= 1.3f) {
//				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
//				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
//			} else if (transform.position.y > 1.3f) {
//				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
//				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
//			} else if (transform.position.y < 1) {
//				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
//				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
//			}
//		}

		AudioSource.PlayClipAtPoint (explosionSound, transform.position);

		GiveScoreAndCoins (this.gameObject.tag);

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
		forceX = 2.5f;

		if (isInitFromBoss) {
			forceX = Random.Range (1, 4) + 0.5f;
		}

		switch (this.gameObject.tag) {
		case "LargestBall":
			forceY = 11.5f;
			break;

		case "LargeBall":
			forceY = 11f;
			break;

		case "MediumBall":
			forceY = 10f;
			break;

		case "SmallBall":
			forceY = 12f;
			break;

		case "SmallestBall":
			forceY = 12f;
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D target)
	{
		if (target.tag == "BottomBrick") {
			myBody.velocity = new Vector2 (0, forceY);
			AudioSource.PlayClipAtPoint (impactSound, transform.position);

		} else if (target.tag == "RightBrick") {
			SetMoveLeft (true);
			AudioSource.PlayClipAtPoint (impactSound, transform.position);

		} else if (target.tag == "LeftBrick") {
			SetMoveRight (true);
			AudioSource.PlayClipAtPoint (impactSound, transform.position);

		} else if (target.tag.Equals ("SoulSword") || target.tag.Equals ("SwordSticky")) {
			GameObject particleSys = InitExplosionParticle ();

			if (Random.Range (1, 10) % 3 == 0) {
				GameObject coinGe = (GameObject)Instantiate (coin, transform.position, Quaternion.identity);
				Destroy (coinGe, 7f);
			}

			if (!gameObject.tag.Equals ("SmallBall")) {
				InitializeBallsAndTurnOffCurrentBall ();

			} else {
				GameplayController.instance.CountSmallBalls ();

				AudioSource.PlayClipAtPoint (explosionSound, transform.position);

				gameObject.SetActive (false);
			}

			Destroy (particleSys, 4f);
		}
	}

	private GameObject InitExplosionParticle ()
	{
		return (GameObject)Instantiate (explosionParticleStone, transform.position, Quaternion.identity);
	}


	void GiveScoreAndCoins (string objTag)
	{

		switch (objTag) {

		case "LargestBall":
			GameplayController.instance.playerScore += Random.Range (600, 700);
			break;

		case "LargeBall":
			GameplayController.instance.playerScore += Random.Range (500, 600);
			break;

		case "MediumBall":
			GameplayController.instance.playerScore += Random.Range (400, 500);
			break;

		case "SmallBall":
			GameplayController.instance.playerScore += Random.Range (300, 400);
			break;

		case "SmallestBall":
			GameplayController.instance.playerScore += Random.Range (200, 300);
			break;
		}
	}


	public void SetIsInitFromBossvFromBoss (bool isInitFromBoss)
	{
		this.isInitFromBoss = isInitFromBoss;
	}
}

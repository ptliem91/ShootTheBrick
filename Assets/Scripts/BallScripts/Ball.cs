using UnityEngine;
using System.Collections;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class Ball : MonoBehaviour
{
	private float forceX, forceY;

	[SerializeField]
	private Rigidbody2D myBody;

	[SerializeField]
	private bool moveLeft, moveRight;

	[SerializeField]
	private GameObject originalBall;

	private GameObject ball1, ball2;
	private Ball ball1Script, ball2Script;

	[SerializeField]
	private AudioClip impactSound, explosionSound;

	[SerializeField]
	private GameObject explosionParticleStone;
	//	private GameObject largestExplosionParticle, largeExplosionParticle, mediumExplosionParticle, smallExplosionParticle, smallestExplosionParticle;

	[SerializeField]
	private GameObject coin;

	// Use this for initialization
	void Awake ()
	{
		if (this.gameObject.tag == "SmallBall") {
			GameplayController.smallBallsCount++;
		}

		SetBallSpeed ();
		InstantiateBalls ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveBall ();
	}

	void InstantiateBalls ()
	{
		if (this.gameObject.tag != "SmallBall") {
			ball1 = Instantiate (originalBall);
			ball2 = Instantiate (originalBall);

//			ball1.name = originalBall.name;
//			ball2.name = originalBall.name;

			ball1Script = ball1.GetComponent<Ball> ();
			ball2Script = ball2.GetComponent<Ball> ();

			ball1.SetActive (false);
			ball2.SetActive (false);
		}
	}

	void InitializeBallsAndTurnOffCurrentBall ()
	{

		Vector3 temp = transform.position;

		ball1.transform.position = temp;
		ball1Script.SetMoveLeft (true);

		ball2.transform.position = temp;
		ball2Script.SetMoveRight (true);

		ball1.SetActive (true);
		ball2.SetActive (true);

		if (gameObject.tag != "SmallBall") {
			if (transform.position.y > 1 && transform.position.y <= 1.3f) {
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
			} else if (transform.position.y > 1.3f) {
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
			} else if (transform.position.y < 1) {
				ball1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
				ball2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
			}
		}

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
//		forceX = Random.Range (1, 4) + 0.5f;

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
		}

		if (target.tag == "RightBrick") {
			SetMoveLeft (true);
			AudioSource.PlayClipAtPoint (impactSound, transform.position);
		}

		if (target.tag == "LeftBrick") {
			SetMoveRight (true);
			AudioSource.PlayClipAtPoint (impactSound, transform.position);
		}

		if (target.tag.Equals ("SoulSword") || target.tag.Equals ("SwordSticky")) {
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
//		switch (this.gameObject.tag) {
//		case "LargestBall":
//			return (GameObject)Instantiate (largestExplosionParticle, transform.position, Quaternion.identity);
//
//		case "LargeBall":
//			return (GameObject)Instantiate (largeExplosionParticle, transform.position, Quaternion.identity);
//
//		case "MediumBall":
//			return (GameObject)Instantiate (mediumExplosionParticle, transform.position, Quaternion.identity);
//
//		case "SmallBall":
//			return (GameObject)Instantiate (smallExplosionParticle, transform.position, Quaternion.identity);
//
//		default:
//			return (GameObject)Instantiate (mediumExplosionParticle, transform.position, Quaternion.identity);
//		}
		return (GameObject)Instantiate (explosionParticleStone, transform.position, Quaternion.identity);
	}


	void GiveScoreAndCoins (string objTag)
	{

		switch (objTag) {

		case "LargestBall":
//			GameplayController.instance.coins += Random.Range (15, 20);
			GameplayController.instance.playerScore += Random.Range (600, 700);
			break;

		case "LargeBall":
//			GameplayController.instance.coins += Random.Range (13, 18);
			GameplayController.instance.playerScore += Random.Range (500, 600);
			break;

		case "MediumBall":
//			GameplayController.instance.coins += Random.Range (11, 16);
			GameplayController.instance.playerScore += Random.Range (400, 500);
			break;

		case "SmallBall":
//			GameplayController.instance.coins += Random.Range (10, 15);
			GameplayController.instance.playerScore += Random.Range (300, 400);
			break;

		case "SmallestBall":
//			GameplayController.instance.coins += Random.Range (9, 14);
			GameplayController.instance.playerScore += Random.Range (200, 300);
			break;

		}

	}

}

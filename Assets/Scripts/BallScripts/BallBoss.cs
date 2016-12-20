using UnityEngine;
using System.Collections;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class BallBoss : MonoBehaviour
{
	private float forceX, forceY;

	[SerializeField]
	private Rigidbody2D myBody;

	[SerializeField]
	private bool moveLeft, moveRight;

	[SerializeField]
	private GameObject originalBall;

	//	private GameObject ball1, ball2;
	//	private BallBoss ball1Script, ball2Script;

	[SerializeField]
	private AudioClip impactSound, explosionSound;

	[SerializeField]
	private GameObject explosionParticleStone, explosionParticleStoneBoss;
	//	private GameObject largestExplosionParticle, largeExplosionParticle, mediumExplosionParticle, smallExplosionParticle, smallestExplosionParticle;

	[SerializeField]
	private GameObject coin;

	public int totalBalls = 10;
	private GameObject[] arrBalls;
	public Ball[] arrBallScripts;

	public int indexInitialBalls = 0;
	public float positionY = 0;

	private float timeAutoInitialBall = 70;
	private bool hasAuto = false;

	// Use this for initialization
	void Awake ()
	{
		if (this.gameObject.tag.Equals ("SmallBall")) {
			GameplayController.smallBallsCount++;
		}

		arrBalls = new GameObject[totalBalls];
		arrBallScripts = new Ball[totalBalls];

		SetBallSpeed ();
		InstantiateBalls ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveBall ();

		if (GameplayController.instance.levelTime <= timeAutoInitialBall && !hasAuto) {
			InitializeBallsAndTurnOffCurrentBall ();
		}
	}

	void InstantiateBalls ()
	{
		if (this.gameObject.tag != "SmallBall") {
			for (int i = 0; i < totalBalls; i++) {
				arrBalls [i] = Instantiate (originalBall);
				arrBallScripts [i] = arrBalls [i].GetComponent<Ball> ();
				arrBalls [i].SetActive (false);
			}
		}
	}

	void InitializeBallsAndTurnOffCurrentBall ()
	{
		hasAuto = true;

		Vector3 temp = transform.position;

		//ball 1
		arrBalls [indexInitialBalls].transform.position = temp;
		arrBallScripts [indexInitialBalls].SetMoveLeft (true);
		arrBallScripts [indexInitialBalls].SetIsInitFromBossvFromBoss (true);
		arrBalls [indexInitialBalls].SetActive (true);
		if (gameObject.tag != "SmallBall") {
			if (transform.position.y > 1 && transform.position.y <= 1.3f) {
				arrBalls [indexInitialBalls].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
			} else if (transform.position.y > 1.3f) {
				arrBalls [indexInitialBalls].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
			} else if (transform.position.y < 1) {
				arrBalls [indexInitialBalls].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
			}
		}

		indexInitialBalls++;

		//ball 2
		if (indexInitialBalls < arrBalls.Length) {
			arrBalls [indexInitialBalls].transform.position = temp;
			arrBallScripts [indexInitialBalls].SetMoveRight (true);
			arrBallScripts [indexInitialBalls].SetIsInitFromBossvFromBoss (true);
			arrBalls [indexInitialBalls].SetActive (true);
			if (gameObject.tag != "SmallBall") {
				if (transform.position.y > 1 && transform.position.y <= 1.3f) {
					arrBalls [indexInitialBalls].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 3.5f);
				} else if (transform.position.y > 1.3f) {
					arrBalls [indexInitialBalls].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 2f);
				} else if (transform.position.y < 1) {
					arrBalls [indexInitialBalls].GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 5.5f);
				}
			}

			indexInitialBalls++;
		}


		AudioSource.PlayClipAtPoint (explosionSound, transform.position);

		GiveScoreAndCoins (this.gameObject.tag);

		///boss ball
		if (indexInitialBalls >= totalBalls) {
			gameObject.SetActive (false);
			InitExplosionParticle (true);
		}

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
		float posY = transform.position.y;

		if (gameObject.tag.Equals ("BossBall")) {
			posY = positionY;
		}

		if (moveLeft) {
			Vector3 temp = transform.position;
			temp.x -= forceX * Time.deltaTime;
			temp.y = posY;
			transform.position = temp;
		}

		if (moveRight) {
			Vector3 temp = transform.position;
			temp.x += forceX * Time.deltaTime;
			temp.y = posY;
			transform.position = temp;
		}
	}

	void SetBallSpeed ()
	{
		forceX = 5f;
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

		} else if (target.tag == "RightBrick") {
			SetMoveLeft (true);
			AudioSource.PlayClipAtPoint (impactSound, transform.position);

		} else if (target.tag == "LeftBrick") {
			SetMoveRight (true);
			AudioSource.PlayClipAtPoint (impactSound, transform.position);

		} else if (target.tag.Equals ("SoulSword") || target.tag.Equals ("SwordSticky")) {
			GameObject particleSys = InitExplosionParticle (false);

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

	private GameObject InitExplosionParticle (bool isBoss)
	{
		if (isBoss) {
			return (GameObject)Instantiate (explosionParticleStoneBoss, transform.position, Quaternion.identity);
		}

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

		case "BossBall":
			GameplayController.instance.playerScore += Random.Range (500, 700);
			break;

		}
	}

}

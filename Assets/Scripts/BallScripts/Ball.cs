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
	private GameObject largestExplosionParticle, largeExplosionParticle, mediumExplosionParticle, smallExplosionParticle, smallestExplosionParticle;

	// Use this for initialization
	void Awake ()
	{
		SetBallSpeed ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveBall ();
	}

	void InstantiateBalls ()
	{
		if (this.gameObject.tag != "SmallestBall") {
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
			forceY = 10.5f;
			break;

		case "MediumBall":
			forceY = 9f;
			break;

		case "SmallBall":
			forceY = 8f;
			break;

		case "SmallestBall":
			forceY = 7f;
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

		if (target.tag == "SoulSword" || target.tag == "SwordSticky") {
			GameObject particleSys = InitExplosionParticle ();

			if (gameObject.tag != "SmallBall") {
				InitializeBallsAndTurnOffCurrentBall ();

			} else {
				AudioSource.PlayClipAtPoint (explosionSound, transform.position);
				gameObject.SetActive (false);
			}

			Destroy (particleSys, 3f);
		}
	}

	private GameObject InitExplosionParticle ()
	{
		switch (this.gameObject.tag) {
		case "LargestBall":
			return (GameObject)Instantiate (largestExplosionParticle, transform.position, Quaternion.identity);

		case "LargeBall":
			return (GameObject)Instantiate (largeExplosionParticle, transform.position, Quaternion.identity);

		case "MediumBall":
			return (GameObject)Instantiate (mediumExplosionParticle, transform.position, Quaternion.identity);

		case "SmallBall":
			return (GameObject)Instantiate (smallExplosionParticle, transform.position, Quaternion.identity);

		default:
			return (GameObject)Instantiate (smallestExplosionParticle, transform.position, Quaternion.identity);
		}
	}

}

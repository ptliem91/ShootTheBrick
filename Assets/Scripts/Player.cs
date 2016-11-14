using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	[SerializeField]
	private GameObject rocket;

	[SerializeField]
	private AudioClip shootingSound;

	[SerializeField]
	private AudioSource backgroundSound;

	public float speed = 8f;
	private float maxVelocity = 4f;

	private Rigidbody2D myBody;
	private Animator anim;

	private bool canShoot;
	private bool canWalk;
	private bool moveLeft = false;

	public UIScript uiScript;

	// Use this for initialization
	void Awake ()
	{
		myBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		canShoot = true;
		canWalk = true;
	}

	void Update ()
	{
		Shoot ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		PlayerWalk ();

	}

	void Shoot ()
	{
		if ((Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space)) && canShoot) {
			canShoot = false;
			StartCoroutine (ShootTheRocket (moveLeft));
		}
	}

	void PlayerWalk ()
	{
		var force = 0f;
		var velocity = Mathf.Abs (myBody.velocity.x);

		float h = Input.GetAxis ("Horizontal");

		if (canWalk) {
			if (h > 0) {
				//moving right
				moveLeft = false;

				if (velocity < maxVelocity) {
					force = speed;
				}

				Vector3 scale = transform.localScale;
				scale.x = 2.5f;

				transform.localScale = scale;
//				transform.rotation = Quaternion.Euler (0, 0, 40);

				anim.SetBool ("HWalking", true);
			} else if (h < 0) {
				//moving left
				moveLeft = true;

				if (velocity < maxVelocity) {
					force = -speed;
				}

				Vector3 scale = transform.localScale;
				scale.x = -2.5f;

				transform.localScale = scale;
//				transform.rotation = Quaternion.Euler (0, 0, -40);

				anim.SetBool ("HWalking", true);
			} else {
				anim.SetBool ("HWalking", false);
			}
		}

		myBody.AddForce (new Vector2 (force, 0));
	}


	IEnumerator ShootTheRocket (bool moveLeft)
	{
		canWalk = false;
		anim.Play ("HeroShooting");

		Vector3 temp = transform.position;
		temp.y += 1f;

//		if (moveLeft) {
//			rocket.transform.rotation = Quaternion.Euler (0, 0, 45);
//		} else {
//			rocket.transform.rotation = Quaternion.Euler (0, 0, -45);
//		}


		Instantiate (rocket, temp, Quaternion.identity);

		AudioSource.PlayClipAtPoint (shootingSound, transform.position);

		yield return new WaitForSeconds (0.16f);
		anim.SetBool ("HShooting", false);
		canWalk = true;

		yield return new WaitForSeconds (0.16f);
		canShoot = true;
	}

	IEnumerator KillThePlayerAndRestartGame ()
	{
		transform.position = new Vector3 (200, 200, 0);

		yield return new WaitForSeconds (1f);

		Application.LoadLevel (Application.loadedLevelName);
	}

	void OnTriggerEnter2D (Collider2D target)
	{
		string[] name = target.name.Split ();

		if (name.Length > 1 && name [1] == "Ball") {
//			StartCoroutine (KillThePlayerAndRestartGame ());
			KillThePlayer ();
		}

//		if (target.tag == "SpawnBall") {
//			StartCoroutine (KillThePlayerAndRestartGame ());
//		}
	}

	void KillThePlayer ()
	{
		anim.SetBool ("HDied", true);

		backgroundSound.Stop ();

		uiScript.FailedGame ();
	}
}

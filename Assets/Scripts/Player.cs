using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	[SerializeField]
	private GameObject rocket;

	[SerializeField]
	private AudioClip shootingSound;

	public float speed = 8f;
	private float maxVelocity = 4f;

	private Rigidbody2D myBody;
	private Animator anim;

	private bool canShoot;
	private bool canWalk;

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
			StartCoroutine (ShootTheRocket ());
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
				if (velocity < maxVelocity) {
					force = speed;
				}

//				Vector3 scale = transform.localScale;
//				scale.x = 1;
//				transform.localScale = scale;

				anim.SetBool ("Walking", true);
			} else if (h < 0) {
				//moving left
				if (velocity < maxVelocity) {
					force = -speed;
				}

//				Vector3 scale = transform.localScale;
//				scale.x = -1;
//				transform.localScale = scale;

				anim.SetBool ("Walking", true);
			} else {
				anim.SetBool ("Walking", false);
			}
		}

		myBody.AddForce (new Vector2 (force, 0));
	}


	IEnumerator ShootTheRocket ()
	{
		canWalk = false;
		anim.Play ("Shooting");

		Vector3 temp = transform.position;
		temp.y += 1f;

		Instantiate (rocket, temp, Quaternion.identity);

		AudioSource.PlayClipAtPoint (shootingSound, transform.position);

		yield return new WaitForSeconds (0.15f);
		anim.SetBool ("Shooting", false);
		canWalk = true;

		yield return new WaitForSeconds (0.15f);
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
			StartCoroutine (KillThePlayerAndRestartGame ());
		}

//		if (target.tag == "SpawnBall") {
//			StartCoroutine (KillThePlayerAndRestartGame ());
//		}
	}
}

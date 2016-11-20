using UnityEngine;
using System.Collections;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class Player : MonoBehaviour
{

	[SerializeField]
	private GameObject rocket;

	[SerializeField]
	private AudioClip shootingSound, deathSound;

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

		anim.SetBool ("HAppear", true);

		//Weapon effect
		Vector3 weaponEffectScale = rocket.transform.localScale;
		weaponEffectScale.x = 0.25f;
		rocket.transform.localScale = weaponEffectScale;

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

	public void Shoot ()
	{
		//(Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space)) && 
		if (canShoot) {
			canShoot = false;
			StartCoroutine (ShootTheRocket ());

			anim.SetBool ("HAppear", false);
		}
	}

	void PlayerWalk ()
	{
		var force = 0f;
		var velocity = Mathf.Abs (myBody.velocity.x);

		float h = Input.GetAxis ("Horizontal");

		if (h > 0 || h < 0) {
			anim.SetBool ("HAppear", false);
		}

		if (canWalk) {

			if (h > 0) {
				//moving right
				moveLeft = false;

				if (velocity < maxVelocity) {
					force = speed;
				}

				Vector3 scale = transform.localScale;
				scale.x = -0.55f;
				transform.localScale = scale;

				//Weapon effect
				Vector3 weaponEffectScale = rocket.transform.localScale;
				weaponEffectScale.x = 0.25f;
				rocket.transform.localScale = weaponEffectScale;

				anim.SetBool ("HWalking", true);
			} else if (h < 0) {
				//moving left
				moveLeft = true;

				if (velocity < maxVelocity) {
					force = -speed;
				}

				Vector3 scale = transform.localScale;
				scale.x = 0.55f;
				transform.localScale = scale;

				//Weapon effect
				Vector3 weaponEffectScale = rocket.transform.localScale;
				weaponEffectScale.x = -0.25f;
				rocket.transform.localScale = weaponEffectScale;

				anim.SetBool ("HWalking", true);
			} else {
				anim.SetBool ("HWalking", false);
			}
		}

		myBody.AddForce (new Vector2 (force, 0));
	}


	IEnumerator ShootTheRocket ()
	{
		canWalk = false;
		anim.Play ("Sk_Attack");

		Vector3 temp = transform.position;
		temp.y += 1f;


		Instantiate (rocket, temp, Quaternion.identity);

		AudioSource.PlayClipAtPoint (shootingSound, transform.position);

		yield return new WaitForSeconds (0.3f);
		anim.SetBool ("HShooting", false);
		canWalk = true;

		yield return new WaitForSeconds (0.3f);
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
		AudioSource.PlayClipAtPoint (deathSound, transform.position);

		anim.Play ("Sk_Died");

		canShoot = false;

		backgroundSound.Stop ();

		uiScript.FailedGame ();
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class Player : MonoBehaviour
{
	public static Player instance;

	[SerializeField]
	private GameObject[] weaponSwords;

	[SerializeField]
	private AudioClip shootingSound, deathSound;

	[SerializeField]
	private AudioSource backgroundSound;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Rigidbody2D myRigidBody;

	[SerializeField]
	private AnimationClip animAttackClip;

	public float speed = 4f;
	private float maxVelocity = 4f;

	private bool canAttack;
	private bool canWalk;
	private bool moveLeft, moveRight;

	private Button shootBtn;

	private GameObject soulSword;

	// Use this for initialization
	void Awake ()
	{
		Time.timeScale = 1f;

		if (instance == null) {
			instance = this;
		}

		shootBtn = GameObject.FindGameObjectWithTag ("ShootButton").GetComponent<Button> ();
		shootBtn.onClick.AddListener (() => PlayerAttack ());

		canAttack = true;
		canWalk = true;

		animator.SetBool ("HAppear", true);

		soulSword = weaponSwords [GameController.instance.selectedWeapon];
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		PlayerWalkKeyBoard ();

		MovePlayer ();
	}

	public void PlayerAttack ()
	{
		if (canAttack && GameplayController.instance.levelInProgress) {
			canAttack = false;
			StartCoroutine (IEAttackAnimation ());

			StopAnimationAppear ();
		}
	}

	void PlayerWalkKeyBoard ()
	{
		var force = 0f;
		var velocity = Mathf.Abs (myRigidBody.velocity.x);

		float h = Input.GetAxis ("Horizontal");

		if (h > 0 || h < 0) {
			StopAnimationAppear ();
		}

		if (canWalk) {

			if (h > 0) {
				//moving right

				if (velocity < maxVelocity) {
					force = speed;
				}

				Vector3 scale = transform.localScale;
				scale.x = -0.5f;
				transform.localScale = scale;

				soulSword.transform.eulerAngles = new Vector2 (0, 0);

				animator.SetBool ("HWalking", true);
			} else if (h < 0) {
				//moving left

				if (velocity < maxVelocity) {
					force = -speed;
				}

				Vector3 scale = transform.localScale;
				scale.x = 0.5f;
				transform.localScale = scale;

				soulSword.transform.eulerAngles = new Vector2 (0, 180);

				animator.SetBool ("HWalking", true);
			} else {
				animator.SetBool ("HWalking", false);
			}
		}

		myRigidBody.AddForce (new Vector2 (force, 0));
	}

	public void StopMoving ()
	{
		moveLeft = moveRight = false;
		animator.SetBool ("HWalking", false);
	}

	public void MovePlayer ()
	{
		if (GameplayController.instance.levelInProgress) {
			if (moveLeft) {
				print ("MovePlayer function ex:: Left ");
				MoveLeft ();
			}

			if (moveRight) {
				print ("MovePlayer function ex:: Right ");
				MoveRight ();
			}
		}
	}

	public void MovePlayerLeft ()
	{
		moveLeft = true;
		moveRight = false;
	}

	public void MovePlayerRight ()
	{
		moveLeft = false;
		moveRight = true;
	}

	public void MoveLeft ()
	{ 
		float force = 0.0f;
		var velocity = Mathf.Abs (myRigidBody.velocity.x);

//		float h = Input.GetAxis ("Horizontal");

		if (canWalk) {
			transform.eulerAngles = new Vector2 (0, 180);

			if (velocity < maxVelocity) {
				force = -speed;
			}

			animator.SetBool ("HWalking", true);
		}
		myRigidBody.AddForce (new Vector2 (force, 0));
	}

	public void MoveRight ()
	{
		float force = 0.0f;
		var velocity = Mathf.Abs (myRigidBody.velocity.x);

//		float h = Input.GetAxis ("Horizontal");

		if (canWalk) {
			transform.eulerAngles = new Vector2 (0, 0);

			if (velocity < maxVelocity) {
				force = speed;
			}

			animator.SetBool ("HWalking", true);
		}
		myRigidBody.AddForce (new Vector2 (force, 0));
	}


	IEnumerator IEAttackAnimation ()
	{
		canWalk = false;
		animator.Play ("Sk_Attack");

		Vector3 temp = transform.position;
		temp.y += 1.5f;
		temp.x += 0.6f;

		Instantiate (soulSword, temp, Quaternion.identity);

		AudioSource.PlayClipAtPoint (shootingSound, transform.position);

		yield return new WaitForSeconds (animAttackClip.length);
		animator.SetBool ("HShooting", false);
		canWalk = true;

		yield return new WaitForSeconds (0.3f);
		canAttack = true;
	}

	IEnumerator IEKillThePlayerAndRestartGame ()
	{
		transform.position = new Vector3 (200, 200, 0);

		yield return new WaitForSeconds (1f);

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	void OnTriggerEnter2D (Collider2D target)
	{
		if (target.name.EndsWith ("Ball") || target.name.EndsWith ("Ball(Clone)")) {
			StartCoroutine (IEKillThePlayer ());
		}
	}

	IEnumerator IEKillThePlayer ()
	{
		AudioSource.PlayClipAtPoint (deathSound, transform.position);

		animator.Play ("Sk_Died");

		canAttack = false;

		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (1.25f));

		GameplayController.instance.PlayerDied ();
	}

	public void StopAnimationAppear ()
	{
		animator.SetBool ("HAppear", false);
	}

	public void PlayAnimationAppear ()
	{
		animator.SetBool ("HAppear", true);
	}
}

using UnityEngine;
using System.Collections;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class EnemyControl : MonoBehaviour
{

	public float speed = 2.0f;

	[SerializeField]
	private GameObject ballParticle;

	[SerializeField]
	private AudioClip popSounds;

	// Update is called once per frame
	void FixedUpdate ()
	{
		GoDown ();
	}

	void GoDown ()
	{
		transform.position += Vector3.down * speed * Time.deltaTime;
	}

	void OnTriggerEnter2D (Collider2D target)
	{
		print (target.tag);
		if (target.tag == "Ground") {
			Destroy (gameObject);
		}

		if (target.tag == "Rocket") {
			GameObject go = Instantiate (ballParticle, transform.position, Quaternion.identity) as GameObject;

			AudioSource.PlayClipAtPoint (popSounds, transform.position);

			Destroy (gameObject);
			Destroy (target.gameObject);
			Destroy (go, 1.5f);
		}
	}
}

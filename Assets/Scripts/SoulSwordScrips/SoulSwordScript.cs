using UnityEngine;
using System.Collections;

/*
 Authored by Liempt - gaucuhanh - ptliem9119@gmail.com
 Copyright 2016-11-20
 */

public class SoulSwordScript : MonoBehaviour
{
	private float soulSpeed = 9f;

	private bool canCutStickySword;

	void Start ()
	{
		canCutStickySword = true;
	}

	void Update ()
	{
		if (this.gameObject.tag == "SwordSticky") {
			if (canCutStickySword) {
				CutSword ();
			}
		} else {
			CutSword ();
		}
	}

	IEnumerator IEResetStickySword ()
	{
		yield return new WaitForSeconds (1.5f);
		if (this.gameObject.tag == "SwordSticky") {
			this.gameObject.SetActive (false);
		}
	}

	public void CutSword ()
	{
		Vector3 temp = transform.position;
		temp.y += soulSpeed * Time.unscaledDeltaTime;
		transform.position = temp;
	}

	void OnTriggerEnter2D (Collider2D target)
	{
		if (target.tag == "TopBrick") {
//			print ("Brick::" + target.tag);
			if (this.gameObject.tag == "SwordSticky") {
//				print ("SoulWord::" + target.tag);
				canCutStickySword = false;

				Vector3 targetPos = target.transform.position;
				Vector3 temp = transform.position;
				targetPos.y -= 0.989f;
				temp.y = targetPos.y;

				transform.position = temp;
				StartCoroutine ("IEResetStickySword");

			} else {
				Destroy (gameObject, 3f);
			}
		}

		if (target.tag.EndsWith ("Ball")) {
			if (this.gameObject.tag == "SwordSticky") {
				StartCoroutine ("IEResetStickySword");
			} else {
				Destroy (gameObject);
			}
		}
	}
}

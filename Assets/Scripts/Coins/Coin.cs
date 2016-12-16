using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{

	private Rigidbody2D myRigidBody;

	// Use this for initialization
	void Start ()
	{
		myRigidBody = GetComponent<Rigidbody2D> ();

	}

	void OnTriggerEnter2D (Collider2D target)
	{
		if (target.tag.Equals ("BottomBrick")) {
			Vector3 temp = target.transform.position;
			temp.y += 0.8f;
			transform.position = new Vector2 (transform.position.x, temp.y);
			myRigidBody.isKinematic = true;

		} else if (target.tag.Equals ("Player")) {
			GameplayController.instance.coins += Random.Range (15, 20);

			Destroy (this.gameObject);
		}

	}
}